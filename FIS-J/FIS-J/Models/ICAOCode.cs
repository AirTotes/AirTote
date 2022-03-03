namespace FIS_J.Models
{
	public static class ICAOCodes
	{
		public static readonly string[] Names = System.Enum.GetNames(typeof(ICAOCode));
		public static readonly ICAOCode[] Codes = System.Enum.GetValues(typeof(ICAOCode)) as ICAOCode[];
	}

	/// <summary>International Civil Aviation Organization Location Inticators</summary>
	public enum ICAOCode
	{
		/// <summary>Tokyo/New Tokyo (Narita)</summary>
		RJAA,

		/// <summary>Matsumoto</summary>
		RJAF,

		/// <summary>Hyakuri (Ibaraki)</summary>
		RJAH,

		/// <summary>Ichigaya</summary>
		RJAI,

		/// <summary>Kasumigaura</summary>
		RJAK,

		/// <summary>Minamitorishima</summary>
		RJAM,

		/// <summary>Niijima</summary>
		RJAN,

		/// <summary>Chichijima</summary>
		RJAO,

		/// <summary>Takigahara</summary>
		RJAT,

		/// <summary>Iwojima</summary>
		RJAW,

		/// <summary>Kozushima</summary>
		RJAZ,

		/// <summary>Kansai intl</summary>
		RJBB,

		/// <summary>Nanki-shirahama</summary>
		RJBD,

		/// <summary>Hiroshimanishi</summary>
		RJBH,

		/// <summary>Kohnan</summary>
		RJBK,

		/// <summary>Tajima</summary>
		RJBT,

		/// <summary>Asahikawa</summary>
		RJCA,

		/// <summary>Obihiro</summary>
		RJCB,

		/// <summary>Sapporo/New Chitose</summary>
		RJCC,

		/// <summary>Sapporo</summary>
		RJCG,

		/// <summary>Hakodate</summary>
		RJCH,

		/// <summary>Chitose</summary>
		RJCJ,

		/// <summary>Kushiro</summary>
		RJCK,

		/// <summary>Memanbetsu</summary>
		RJCM,

		/// <summary>Nakashibetsu</summary>
		RJCN,

		/// <summary>Sapporo/Okadama</summary>
		RJCO,

		/// <summary>Rebun</summary>
		RJCR,

		/// <summary>Kushiro/Kenebetsu</summary>
		RJCS,

		/// <summary>Tokachi</summary>
		RJCT,

		/// <summary>Wakkanai</summary>
		RJCW,

		/// <summary>Muroran/Yakumo</summary>
		RJCY,

		/// <summary>Amakusa</summary>
		RJDA,

		/// <summary>Iki</summary>
		RJDB,

		/// <summary>Yamaguchi-Ube</summary>
		RJDC,

		/// <summary>Fukuoka</summary>
		RJDG,

		/// <summary>Kamigoto</summary>
		RJDK,

		/// <summary>Metabaru</summary>
		RJDM,

		/// <summary>Ojika</summary>
		RJDO,

		/// <summary>Tsushima</summary>
		RJDT,

		/// <summary>Monbetsu</summary>
		RJEB,

		/// <summary>Asahikawa</summary>
		RJEC,

		/// <summary>Okushiri</summary>
		RJEO,

		/// <summary>Rishiri Island</summary>
		RJER,

		/// <summary>Ashiya</summary>
		RJFA,

		/// <summary>Yakushima</summary>
		RJFC,

		/// <summary>Fukue</summary>
		RJFE,

		/// <summary>Fukuoka</summary>
		RJFF,

		/// <summary>Tanegashima</summary>
		RJFG,

		/// <summary>Kagoshima</summary>
		RJFK,

		/// <summary>Miyazaki</summary>
		RJFM,

		/// <summary>Nyutabaru</summary>
		RJFN,

		/// <summary>Oita</summary>
		RJFO,

		/// <summary>Kitakyushu</summary>
		RJFR,

		/// <summary>Saga</summary>
		RJFS,

		/// <summary>Kumamoto</summary>
		RJFT,

		/// <summary>Nagasaki</summary>
		RJFU,

		/// <summary>Sasebo usn base</summary>
		RJFW,

		/// <summary>Kanoya</summary>
		RJFY,

		/// <summary>Tsuiki</summary>
		RJFZ,

		/// <summary>Fukuoka/Jcab Air Traffic</summary>
		RJJJ,

		/// <summary>Amami</summary>
		RJKA,

		/// <summary>Okierabu</summary>
		RJKB,

		/// <summary>Kikai/Kikaigashima Island</summary>
		RJKI,

		/// <summary>Tokunoshima Island</summary>
		RJKN,

		/// <summary>Fukui</summary>
		RJNF,

		/// <summary>Gifu</summary>
		RJNG,

		/// <summary>Hamamatsu</summary>
		RJNH,

		/// <summary>Kanazawa/Komatsu</summary>
		RJNK,

		/// <summary>Nagoya</summary>
		RJNN,

		/// <summary>Oki</summary>
		RJNO,

		/// <summary>Toyama</summary>
		RJNT,

		/// <summary>Yaizu/Shizuhama</summary>
		RJNY,

		/// <summary>New hiroshima</summary>
		RJOA,

		/// <summary>Okayama</summary>
		RJOB,

		/// <summary>Izumo</summary>
		RJOC,

		/// <summary>Akeno</summary>
		RJOE,

		/// <summary>Hofu</summary>
		RJOF,

		/// <summary>Miho</summary>
		RJOH,

		/// <summary>Iwakuni</summary>
		RJOI,

		/// <summary>Kochi</summary>
		RJOK,

		/// <summary>Matsuyama</summary>
		RJOM,

		/// <summary>Osaka/Intl</summary>
		RJOO,

		/// <summary>Komatsujima</summary>
		RJOP,

		/// <summary>Tottori</summary>
		RJOR,

		/// <summary>Tokushima</summary>
		RJOS,

		/// <summary>Takamatsu</summary>
		RJOT,

		/// <summary>Iwami</summary>
		RJOW,

		/// <summary>Osaka/Yao</summary>
		RJOY,

		/// <summary>Ozuki</summary>
		RJOZ,

		/// <summary>Aomori</summary>
		RJSA,

		/// <summary>Yamagata</summary>
		RJSC,

		/// <summary>Sado</summary>
		RJSD,

		/// <summary>Fukushima</summary>
		RJSF,

		/// <summary>Hachinohe</summary>
		RJSH,

		/// <summary>Hanamaki</summary>
		RJSI,

		/// <summary>Akita</summary>
		RJSK,

		/// <summary>Misawa</summary>
		RJSM,

		/// <summary>Niigata</summary>
		RJSN,

		/// <summary>Ominato</summary>
		RJSO,

		/// <summary>Odate-Noshiro</summary>
		RJSR,

		/// <summary>Sendai</summary>
		RJSS,

		/// <summary>Matsushima</summary>
		RJST,

		/// <summary>Kasuminome</summary>
		RJSU,

		/// <summary>Shonai</summary>
		RJSY,

		/// <summary>Atsugi</summary>
		RJTA,

		/// <summary>Tachikawa</summary>
		RJTC,

		/// <summary>Tokyo (City)</summary>
		RJTD,

		/// <summary>Tateyama</summary>
		RJTE,

		/// <summary>Chofu</summary>
		RJTF,

		/// <summary>Tokyo (Acc)</summary>
		RJTG,

		/// <summary>Hachiojima</summary>
		RJTH,

		/// <summary>Tokyo (Hel)</summary>
		RJTI,

		/// <summary>Iruma</summary>
		RJTJ,

		/// <summary>Kisarazu</summary>
		RJTK,

		/// <summary>Shimofusa</summary>
		RJTL,

		/// <summary>Oshima</summary>
		RJTO,

		/// <summary>Miyakehima</summary>
		RJTQ,

		/// <summary>Zama/Kastner</summary>
		RJTR,

		/// <summary>Tokyo/Intl</summary>
		RJTT,

		/// <summary>Utsunomiya</summary>
		RJTU,

		/// <summary>Zama</summary>
		RJTW,

		/// <summary>Yokota</summary>
		RJTY,

		/// <summary>Fuchu</summary>
		RJTZ,

		/// <summary>Naha</summary>
		ROAH,

		/// <summary>Iejima (Usaf Base)</summary>
		RODE,

		/// <summary>Kadena ab</summary>
		RODN,

		/// <summary>Ishigaki jima</summary>
		ROIG,

		/// <summary>Kumejima</summary>
		ROKJ,

		/// <summary>Kerama</summary>
		ROKR,

		/// <summary>Yomitan</summary>
		ROKW,

		/// <summary>Minamidaito jima</summary>
		ROMD,

		/// <summary>Miyako</summary>
		ROMY,

		/// <summary>Aguni</summary>
		RORA,

		/// <summary>Iejima (Civil)</summary>
		RORE,

		/// <summary>Naha (Acc)</summary>
		RORG,

		/// <summary>Hateruma</summary>
		RORH,

		/// <summary>Kitadaito</summary>
		RORK,

		/// <summary>Shimojishima</summary>
		RORS,

		/// <summary>Tarama</summary>
		RORT,

		/// <summary>Yoron</summary>
		RORY,

		/// <summary>Futema</summary>
		ROTM,

		/// <summary>Yonaguni jima</summary>
		ROYN,
	}
}
