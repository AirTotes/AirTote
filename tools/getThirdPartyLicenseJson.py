from dataclasses import asdict, dataclass
import json
from subprocess import PIPE, Popen
from os.path import dirname
from sys import argv
from typing import List
from xml.etree import ElementTree
from aiohttp import ClientSession
import asyncio

CSPROJ_PATH = dirname(__file__) + "/../AirTote/AirTote.csproj"
TIMEOUT_SEC = 2
ENC = "utf-8"
GET_LICENSE_INFO_BASE_URL = "https://api.nuget.org/v3-flatcontainer/"

LICENSE_INFO_LIST_FILE_NAME = "third_party_license_list.json"

@dataclass
class PackageInfo:
  PackageName: str
  ResolvedVersion: str

@dataclass
class LicenseInfo:
  id: str
  version: str
  license: str
  licenseUrl: str
  author: str
  projectUrl: str
  copyrightText: str

async def getLicenseInfo(session: ClientSession, requestBaseUrl: str, packageInfo: PackageInfo) -> LicenseInfo:
  packageNameLower = str.lower(packageInfo.PackageName)
  resourceUrl = f'{requestBaseUrl}{packageNameLower}/{packageInfo.ResolvedVersion}/{packageNameLower}.nuspec' 
  metadata: ElementTree.Element = None
  NUSPEC_XML_NAMESPACE = {}
  async with session.get(resourceUrl) as stream:
    root = ElementTree.fromstring(await stream.text())
    if root is None:
      return None
    # namespaceがパッケージによって違う場合があるため、動的に取得する
    if root.tag.find('{') >= 0:
      NUSPEC_XML_NAMESPACE[''] = root.tag.lstrip('{').rstrip('}package')
    metadata = root.find("metadata", NUSPEC_XML_NAMESPACE)
  
  return LicenseInfo(
    metadata.findtext("id", namespaces=NUSPEC_XML_NAMESPACE),
    metadata.findtext("version", namespaces=NUSPEC_XML_NAMESPACE),
    metadata.findtext("license", namespaces=NUSPEC_XML_NAMESPACE),
    metadata.findtext("licenseUrl", namespaces=NUSPEC_XML_NAMESPACE),
    metadata.findtext("authors", namespaces=NUSPEC_XML_NAMESPACE)
    or metadata.findtext("owners", namespaces=NUSPEC_XML_NAMESPACE),
    metadata.findtext("projectUrl", namespaces=NUSPEC_XML_NAMESPACE),
    metadata.findtext("copyright", namespaces=NUSPEC_XML_NAMESPACE),
  )

async def main(targetFramework: str, targetDir: str) -> int:
  lines: List[List[bytes]]
  with Popen(["dotnet", "list", CSPROJ_PATH, "package", "--framework", targetFramework, '--include-transitive'], stdout=PIPE) as p:
    lines = [line.split() for line in p.stdout.readlines()]
    if len(lines) <= 3:
      return 1
    if p.wait(TIMEOUT_SEC) != 0:
      return p.returncode
  lines = lines[3:]

  packages: List[PackageInfo] = []
  for v in lines:
    if len(v) <= 0 or v[0] != b'>':
      continue
    packages.append(PackageInfo(v[1].decode(ENC), v[-1].decode(ENC)))
  
  packageInfoList: List[PackageInfo]
  async with ClientSession() as session:
    packageInfoList = await asyncio.gather(*[getLicenseInfo(session, GET_LICENSE_INFO_BASE_URL, v) for v in packages])
  
  with open(f'{targetDir}/{LICENSE_INFO_LIST_FILE_NAME}', 'w') as f:
    json.dump([asdict(v) for v in packageInfoList], f)

  return 0


if __name__ == "__main__":
  if len(argv) <= 1:
    exit(1)
  exitCode = asyncio.run(main(argv[1], argv[2]))
  exit(exitCode)