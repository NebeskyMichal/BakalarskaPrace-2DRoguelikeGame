# Cards 'n' Slimes

Tento repozitář obsahuje zdrojové kódy a kompletní projekt 2D karetní roguelike hry s názvem Cards 'n' Slimes. Aplikace vznikla jako praktická část bakalářské práce „Návrh a vývoj 2D hry s prvky roguelike v enginu Unity s využitím jazyka C#“ obhájené na Fakultě informatiky a statistiky Vysoké školy ekonomické v Praze (VŠE FIS).

## O projektu

Hra je plně vyvinuta v enginu Unity a zaměřuje se na implementaci tradičních roguelike prvků do 2D prostředí s využitím mechanik budování balíčku (deck-building). Hlavním cílem je zprostředkovat komplexní strategický zážitek, kde hráč čelí permanentní smrti (permadeath) a postupuje procedurálně generovanou mapou. 

## Klíčové herní mechaniky

* **Strategická smyčka a procedurální generování:** Mapa je generována jako orientovaný acyklický graf (DAG), což zaručuje plynulý postup bez zacyklení směrem k závěrečnému bossovi. Cesty se dynamicky větví a obsahují různé herní události, jako jsou běžné a elitní souboje, obchody či táboráky pro léčení.
* **Taktický tahový souboj:** Soubojový systém je postaven na vykládání karet z balíčku za využití omezeného zdroje energie (many).
* **Systém záměrů (Intent System):** Nepřátelé využívají systém predikce tahů s váženým náhodným výběrem akcí. Hráč tak předem vidí, jaký útok nebo efekt nepřítel chystá, což umožňuje činit informovaná taktická rozhodnutí a eliminuje prvek čisté náhody.

## Architektura a technologie

Projekt byl navržen s vysokým důrazem na modularitu a čistý kód.

* **Engine a vývojové prostředí:** Projekt je postaven na enginu Unity (verze 6000.3.6f1) a logika je programována v jazyce C# prostřednictvím IDE JetBrains Rider.
* **Oddělení dat od logiky (Decoupling):** Veškerá data o kartách, nepřátelích a herních událostech jsou zapouzdřena a spravována pomocí technologie ScriptableObjects. To optimalizuje využití paměti a umožňuje snadné úpravy herního balancu přímo v editoru bez zásahu do zdrojového kódu.
* **Architektura řízená událostmi:** Zpracování tahového souboje zajišťuje robustní `ActionSystem`, který kombinuje návrhové vzory Command a Observer a asynchronně řetězí akce a reakce pomocí mechanismu Coroutines.
* **Persistence dat:** Plynulý přechod mezi scénami (`MainMenu`, `MapScene`, `FightScene`) a uchování stavu průchodu zajišťuje správce `RunManagerSystem` implementovaný za pomoci návrhového vzoru Persistent Singleton.
* **Grafika a animace:** Vizuální stránka využívá pixel art vytvořený v grafickém editoru Aseprite. Pro plynulé animace karet napříč UI byla integrována knihovna DOTween a pro správu efektů v editoru balíček Serialize Reference Editor.

## Stažení a dokumentace

* **Hratelný build a Text bakalářské práce:** Zkompilovanou spustitelnou verzi hry pro Windows a text samotné práce naleznete v sekci [Releases](https://github.com/NebeskyMichal/BakalarskaPrace-2DRoguelikeGame/releases).

## Spuštění projektu v Unity

Pro otevření a kompilaci zdrojových kódů je vyžadován Unity Hub a editor verze `6000.3.6f1`.
