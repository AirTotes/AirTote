# MapIcons

AirToteの地図に使用するアイコンの一覧です。

## 設計

アイコン全体としてのサイズは`48x48`として、
タップ領域の確保のため「半径24 白色塗りつぶし 透過度0.01」の円をバックに配置し、
アイコンのコンテンツは20x20に収める。

## アイコン一覧

ファイル名中「`{RP}`」には「`CRP`(Compulsory Reporting Point = 義務位置通報点)」または「`ORRP`(On Request Reporting Point = 非義務位置通報点)」が入ります。

位置通報点のアイコンの見分け方は、「黒の塗り潰しが多い方が`CRP`」「黒の塗り潰しが少ない方が`ORRP`」です。
表中の`Image`行では、左 (または 上) に`CRP`のアイコン、右 (または 下) に`ORRP`のアイコンを配置しています。

| File Name | Image | Description |
| :---: | :---: | :---|
| `flight.svg` | ![飛行機の画像](./flight.svg) | 飛行機の画像。 Material Symbolsから一部修正して使用 |
| `REP.{RP}.svg` | ![VFR Reporting Point Icon (Compulsory Reporting Point)](./REP.CRP.svg) ![VFR Reporting Point Icon (On Request Reporting Point)](./REP.ORRP.svg) | VFR飛行時の位置通報点のアイコン |
| `TACAN.{RP}.svg` | ![UHF Tactical Air Navigation Aid Icon (Compulsory Reporting Point)](./TACAN.CRP.svg) ![UHF Tactical Air Navigation Aid Icon (On Request Reporting Point)](./TACAN.ORRP.svg) | TACAN(戦術航法装置)のアイコン |
| `VOR.{RP}.svg` | ![VHF Omnidirectional Radio Range Icon (Compulsory Reporting Point)](./VOR.CRP.svg) ![VHF Omnidirectional Radio Range Icon (On Request Reporting Point)](./VOR.ORRP.svg) | VOR(超短波全方向式無線標識)のアイコン |
| `VORDME.{RP}.svg` | ![Collocated VOR and DME Icon (Compulsory Reporting Point)](./VORDME.CRP.svg) ![Collocated VOR and DME Icon (On Request Reporting Point)](./VORDME.ORRP.svg) | VOR/DMEのアイコン |
| `VORTAC.{RP}.svg` | ![Collocated VOR and TACAN Icon (Compulsory Reporting Point)](./VORTAC.CRP.svg) ![Collocated VOR and TACAN Icon (On Request Reporting Point)](./VORTAC.ORRP.svg) | VORTACのアイコン |
