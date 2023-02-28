from aiohttp import ClientSession, ClientTimeout, TCPConnector
from dataclasses import dataclass
from sys import argv, stderr
from typing import List
from tqdm import tqdm

import csv

import asyncio


@dataclass
class LinkCheckResult:
	i: int
	Name: str
	URL: str
	IsValid: bool
	Message: str

	def str(self) -> str:
		_str: str = f'[{self.i}]: {self.Name}\n\t... {"OK" if self.IsValid else "NG"}'
		if not self.IsValid:
			_str += f'\n\tURL: {self.URL}'
			_str += f'\n\tMsg: {self.Message}'
		return _str


async def checkLink(i: int, session: ClientSession, name: str, url: str) -> LinkCheckResult:
	isValid: bool = False
	message: str = ''

	if url is None or url == '':
		message = 'URL is None or empty'
	else:
		try:
			async with session.head(url) as result:
				if result.ok:
					isValid = True
				else:
					message = str(result.reason)
		except BaseException as ex:
			message = str(ex)
	return LinkCheckResult(i, name, url, isValid, message)


async def main(csv_file_path: str) -> int:
	with open(csv_file_path, 'r') as f:
		reader = csv.reader(f)

		result_list: List[LinkCheckResult] = []
		async with ClientSession(
			connector = TCPConnector(limit = 5, force_close = True),
			timeout = ClientTimeout(sock_read = 5)
		) as session:
			for task in tqdm(asyncio.as_completed([checkLink(i, session, row[2], row[3]) for i, row in enumerate(reader)])):
				result_list.append(await task)

		for v in sorted(result_list, key = lambda v: v.i):
			print(v.str())
	return 0

if __name__ == "__main__":
	if len(argv) <= 1:
		print("too few arguments", file=stderr)
		exit(1)
	exitCode = asyncio.run(main(argv[1]))
	exit(exitCode)
