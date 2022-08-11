from dataclasses import asdict, dataclass
import json
from subprocess import PIPE, Popen
from os.path import dirname
from sys import argv, stderr
from typing import List
from xml.etree import ElementTree
from aiofiles import open as aio_open
import asyncio

CSPROJ_PATH = dirname(__file__) + "/../AirTote/AirTote.csproj"
TIMEOUT_SEC = 2
ENC = "utf-8"

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
  licenseDataType: str
  licenseUrl: str
  author: str
  projectUrl: str
  copyrightText: str

def getNugetGlobalPackagesDir() -> str:
  with Popen(["dotnet", "nuget", "locals", "global-packages", "-l"], stdout=PIPE) as p:
    execResult = p.stdout.readlines()[0].decode(ENC)
    return execResult.removeprefix("global-packages: ").removesuffix('\n')

async def getLicenseInfo(globalPackagesDir: str, packageInfo: PackageInfo) -> LicenseInfo:
  packageNameLower = str.lower(packageInfo.PackageName)
  resourcePath = f'{globalPackagesDir}{packageNameLower}/{packageInfo.ResolvedVersion}/{packageNameLower}.nuspec'

  metadata: ElementTree.Element = None
  NUSPEC_XML_NAMESPACE = {}
  async with aio_open(resourcePath, 'r') as stream:
    root = ElementTree.fromstring(await stream.read())
    if root is None:
      return None
    # namespaceがパッケージによって違う場合があるため、動的に取得する
    if root.tag.find('{') >= 0:
      NUSPEC_XML_NAMESPACE[''] = root.tag.removeprefix('{').removesuffix('}package')
    metadata = root.find("metadata", NUSPEC_XML_NAMESPACE)
  
  licenseElem = metadata.find("license", namespaces=NUSPEC_XML_NAMESPACE)
  licenseText: str = None
  licenseDataType: str = None
  if licenseElem is not None:
    licenseText = licenseElem.text
    licenseDataType = licenseElem.attrib['type']

  return LicenseInfo(
    metadata.findtext("id", namespaces=NUSPEC_XML_NAMESPACE),
    metadata.findtext("version", namespaces=NUSPEC_XML_NAMESPACE),
    licenseText,
    licenseDataType,
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
  
  globalPackagesDir = getNugetGlobalPackagesDir()
  packageInfoList = await asyncio.gather(*[getLicenseInfo(globalPackagesDir, v) for v in packages])
  
  with open(f'{targetDir}/{LICENSE_INFO_LIST_FILE_NAME}', 'w') as f:
    json.dump([asdict(v) for v in packageInfoList], f)

  return 0


if __name__ == "__main__":
  if len(argv) <= 2:
    print("too few arguments", file=stderr)
    exit(1)
  exitCode = asyncio.run(main(argv[1], argv[2]))
  exit(exitCode)
