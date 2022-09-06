from math import sqrt
from sys import argv, stderr

CENTER = 10.0
SQRT3 = sqrt(3)

def getTacanIconPathData(rectHeight: float, hexLineLength: float) -> str:
	HalfOfRectHeight_BaseTriangle_Height = rectHeight * SQRT3 / 2
	yOffset = CENTER - ((rectHeight / 2) + ((((1.5 + SQRT3) * rectHeight) + (hexLineLength * SQRT3)) / 3))

	# Upper Rect
	URect_FromCenter_Bottom_X = hexLineLength + rectHeight
	URect_Bottom_Y = yOffset + (2 * rectHeight)

	URect_FromCenter_Left_X = hexLineLength + rectHeight + HalfOfRectHeight_BaseTriangle_Height
	URect_Left_Y = yOffset + (3 * rectHeight / 2)

	URect_FromCenter_Top_X = hexLineLength + HalfOfRectHeight_BaseTriangle_Height
	URect_Top_Y = yOffset

	URect_FromCenter_Right_X = hexLineLength
	URect_Right_Y = yOffset + (rectHeight / 2)

	# Upper Left Rect
	ULRect = ''
	ULRect += f' {CENTER - URect_FromCenter_Bottom_X},{URect_Bottom_Y}'
	ULRect += 'L'
	ULRect += f' {CENTER - URect_FromCenter_Left_X},{URect_Left_Y}'
	ULRect += f' {CENTER - URect_FromCenter_Top_X},{URect_Top_Y}'
	ULRect += f' {CENTER - URect_FromCenter_Right_X},{URect_Right_Y}'

	# Upper Right Rect
	URRect = ''
	URRect += f' {CENTER + URect_FromCenter_Right_X},{URect_Right_Y}'
	URRect += 'L'
	URRect += f' {CENTER + URect_FromCenter_Top_X},{URect_Top_Y}'
	URRect += f' {CENTER + URect_FromCenter_Left_X},{URect_Left_Y}'
	URRect += f' {CENTER + URect_FromCenter_Bottom_X},{URect_Bottom_Y}'

	# Bottom Rect
	BRect = ''
	BRect_Top_Y = yOffset + (2 * rectHeight) + (hexLineLength * SQRT3)
	BRect_Bottom_Y = BRect_Top_Y + rectHeight
	BRect += f' {CENTER + rectHeight},{BRect_Top_Y}'
	BRect += 'L'
	BRect += f' {CENTER + rectHeight},{BRect_Bottom_Y}'
	BRect += f' {CENTER - rectHeight},{BRect_Bottom_Y}'
	BRect += f' {CENTER - rectHeight},{BRect_Top_Y}'

	# Hex
	Hex = ''
	Hex += f' {CENTER - URect_FromCenter_Bottom_X},{URect_Bottom_Y}'
	Hex += 'L'
	Hex += f' {CENTER - URect_FromCenter_Right_X},{URect_Right_Y}'
	Hex += f' {CENTER + URect_FromCenter_Right_X},{URect_Right_Y}'
	Hex += f' {CENTER + URect_FromCenter_Bottom_X},{URect_Bottom_Y}'
	Hex += f' {CENTER + rectHeight},{BRect_Top_Y}'
	Hex += f' {CENTER - rectHeight},{BRect_Top_Y}'

	Outline = f'M{ULRect} {URRect} {BRect}Z'

	return (
		Outline,
		f'M{ULRect}Z',
		f'M{URRect}Z',
		f'M{BRect}Z',
		f'M{Hex}Z',
		URect_FromCenter_Left_X * 2,
		BRect_Bottom_Y - yOffset,
		yOffset
	)

if __name__ == '__main__':
	if len(argv) <= 1:
		print('getTacanIconPathData {rectHeight} {hexLineLength} [{yOffset}]', file=stderr)
		exit(1)

	rectHeight = float(argv[1])
	hexLineLen = float(argv[2])

	(Outline, ULRect, URRect, BRect, Hex, Width, Height, yOffset) = getTacanIconPathData(rectHeight, hexLineLen)
	print(
f'''<path
	stroke="black"
	d="{Outline}"/>
<svg
	fill="none">
	<path d="{ULRect}"/>
	<path d="{URRect}"/>
	<path d="{BRect}"/>
</svg>
<path
	fill="none"
	d="{Hex}"/>''')
	print('', file=stderr)
	print(f'  Width: {Width}', file=stderr)
	print(f' Height: {Height}', file=stderr)
	print(f'yOffset: {yOffset}', file=stderr)
