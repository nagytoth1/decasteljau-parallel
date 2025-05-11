# Szoftverfejlesztés párhuzamos architektúrákra

**Nagy-Tóth Bence DZKBX0**

## A DeCasteljau algoritmus párhuzamosítása

A DeCasteljau-algoritmus a Bézier-görbék egyik legfontosabb numerikus számítási módszere, amelyet széles körben használnak a számítógépes grafikában és a számítógépes modellezésben. Ez az eljárás képes egy Bézier-görbe adott t paraméter szerinti pontját kiszámítani az iteratív kontrollpontok interpolálásával.

Az algoritmus a kontrollpontok között lineáris interpolációt végez. Ezáltal elkerülhetjük azokat a problémákat, amelyek a klasszikus polinomiális számítások során előfordulhatnak.

A DeCasteljau-algoritmus és a Bernstein-polinomok matematikailag azonos görbét írnak le, eltérő megközelítéssel. A DeCasteljau-algoritmus egy interpolációs eljárás, amely a kontrollpontok között lineáris interpolációt végez. Minden lépésben a kontrollpontok közötti interpoláció eredményeként csökken a bemenő pontok száma, amíg végül egyetlen pontot nem kapunk, amely a kívánt t paraméter szerinti pontot adja meg nekünk a görbén. Ezzel szemben a Bernstein-polinomok egy algebrai módszert alkalmaznak, ahol a görbét egy explicit polinomiális kifejezés segítségével definiálják.

A Bézier-görbéknél a kontrollpontok kulcsfontosságú szerepet játszanak, hiszen ezek határozzák meg a görbe alakját és geometriáját. Fontos megjegyezni, hogy a kontrollpontok nem feltétlenül esnek a görbére, hanem azok irányítják azt, mintha egy láthatatlan erő húzná őket a görbe irányába.

A DeCasteljau algoritmus különböző megvalósításait C# nyelven írtam meg, egy DLL-állomány készült ezekből, amelynek metódusait egy Windows Forms-os grafikus felületen, illetve egy konzolos alkalmazásból használtam. Féléves feladatom forráskódja [ezen a Github linken](https://github.com/nagytoth1/decasteljau-parallel.git) keresztül elérhető.

Három eltérő megoldást készítettem a DeCasteljau algoritmus párhuzamosításra. Minden módszer egy adott stratégiát alkalmaz a számítások párhuzamosítására, különböző szinkronizálási és teljesítmény-optimalizációs technikákkal.

## GraphicsDLL állomány

Ez a DLL projekt tartalmazza a különféle megvalósításokat a DeCasteljau algoritmus futtatására, amelyeket a Factory és Strategy tervezési minták használatával rendszereztem. A célom alapvetően az volt, hogy a kódot karbantarthatóvá és könnyen bővíthetővé tegyem, hogy a későbbiekben egyszerűbben hozzáadhassunk újabb végrehajtási módokat anélkül, hogy a többi, ettől függő logikát módosítani kellene.

A Factory minta segít az algoritmusok típusának dinamikus kiválasztásában és létrehozásában. A Strategy minta pedig lehetővé teszi, hogy különböző végrehajtási módszereket (pl. iteratív, párhuzamos, rekurzív-párhuzamos) válasszunk ki és használjunk dinamikusan.

### A GraphicsDLL projekt könyvtárstruktúrája

A következő könyvtárstruktúrát hoztam létre a jobb átláthatóság miatt:

![alt text](image-1.png)

- **factory**: A `DeCasteljauFactory.cs` felelős a végrehajtási stratégiák példányosításáért, míg a `DeCasteljauStrategies.cs` enum típusként tartalmazza a különböző elérhető stratégiai osztályokat, amelyek között a felhasználó választhat.

- **strategy**: Strategy pattern megvalósítását tartalmazó könyvtár, a DeCasteljau algoritmusok különböző megvalósításai itt találhatóak.

  - **abstract**: Absztrakt osztályokat tartalmazó könyvtár. Az absztrakt `DeCasteljauStrategy.cs` osztály tartalmazza a DeCasteljau algoritmus bemeneti paramétereit, illetve egy `Iterate` függvényt, amelyet a konkrét implementációs gyermekosztályoknak kell implementálniuk. További két absztrakt osztály is megtalálható itt (`IterativeDeCasteljau.cs` és a `RecursiveDeCasteljau.cs`), mivel voltak a rekurzív illetve az iteratív implementációknak közös, átfedő függvényei, kódduplikáció elkerülése miatt öröklődéssel adtam le az implementáló osztályoknak.

  - **implementation**: Az egyes algoritmusok konkrét implementáló osztályai találhatóak ebben a könyvtárban.

- **utilities**: A `ExtendedGraphics.cs` osztály segít a grafikus megjelenítésben, biztosítva a DeCasteljau algoritmus lépéseinek vizualizációját.

Volt egy olyan pont a fejlesztés során, amikor szükségessé vált a DeCasteljau algoritmusok átalakítása azért, hogy ténylegesen mérni tudjuk a párhuzamosítás hatásosságát. A célom az volt, lehetőséget adjak a `GraphicsDLL` használatára a CLI (parancssori) és Windows Forms felületek felől. Ekkor jól jött a Factory és Strategy tervezési minták adta előny, a rendszerezett osztálystruktúra segített abban, hogy csak ott kellett módosítanom az implementációimat, ahol ténylegesen szükséges volt, a felhasználó, már meglévő Form kódja nem borult fel teljesen például, csak az absztrakt hívások módosulásainak volt kitéve.

### Párhuzamosítási módszerek

Párhuzamosítási ötlet: Egy-egy adott t-értékekkel meghívott DeCasteljau-függvénnyel egy-egy pontot kapunk meg a görbén. Ha a t értékeit 0-tól 1-ig bejárjuk, 10, 100 vagy 1000 lépésben, egyre finomabb felbontású görbét kapunk. Mivel ezek a lefutások függetlenek egymástól, ideálisak párhuzamos végrehajtásra.

A rajzolási műveletek párhuzamosítása elkerülhető azzal, hogy azokat a párhuzamosított résztől elkülönítjük, így a párhuzamos szálak feladata csak a görbe egyes pontjainak kiszámítására redukálódik, ezért egy `lock(g)` zárolás megspórolható. A rajzolási műveleteket a görbe kiszámított pontokra utólag, szekvenciálisan végezzük.

<!-- todo: befejezni -->

1. **IterativeParallelDeCasteljau**

- A párhuzamos végrehajtás a Parallel.For ciklus használatával történik, amely lehetővé teszi, hogy több szálon dolgozzunk az egyes t-értékek kiszámításán. Mivel az egyes iterációk függetlenek egymástól, így könnyen párhuzamosíthatók.

- Az algoritmus a tömböt kisebb szeletekre osztja. Jelenleg dinamikusan ProcessorCount \* 2 résztömbbel számol, így a résztömbök mérete az iterációk számától függ, tehát például ha 100 iterációs lépésre van szükség, és a processzormagok száma 2, akkor 4-felé bontva 25-ös méretű tömbök jönnek létre.

- Ez a módszer javítja a párhuzamos végrehajtás hatékonyságát, mivel minden szál egy-egy darabra koncentrál, és biztosítja, hogy nem aprózódnak el túlságosan a feladatok (korábban konstans 5-re volt állítva a résztömbök mérete).

2. **IterativeTPLDeCasteljau**

- iteratív DeCasteljau algoritmust párhuzamosít Task Parallel Library segítségével
- Task.Run() segítségével minden aktuális résztömbre új Taskot indítottam, az ily módon létrehozott Task objektumokat listába gyűjtöttem.

- A futtatott Task objektumok eredményeit bevárom, majd a függvény végén visszatérek az eredményekkel.

3. **RecursiveParallelDeCasteljau**

- rekurzív DeCasteljau algoritmust párhuzamosít Task Parallel Library segítségével
- Task.Run() segítségével minden aktuális résztömbre új Taskot indítottam, az ily módon létrehozott Task objektumokat listába gyűjtöttem.
- A futtatott Task objektumok eredményeit bevárom, majd a függvény végén visszatérek az eredményekkel.

A Task Parallel Library (TPL) használata előnyösebb volt a párhuzamosítási megoldásban, mint a szálak kezelését egyesével végezni, mivel a TPL a ThreadPool-t is képes kihasználni. Ez jelentősen csökkenti a szálkezeléssel kapcsolatos overheadet és javítja a rendszer erőforrásainak kihasználását. A TPL automatikusan kezeli a szálakat, és biztosítja, hogy a rendszer optimálisan oszthassa el a feladatokat, így a párhuzamosítás hatékonyabbá válik, különösen nagy számú iterációk vagy kontrollpontok esetén.

Az eredmény mindhárom párhuzamosított módszer esetében megegyezik: egy görbe pontjait tartalmazó tömböt kapunk, amely tömböt a Graphics osztály segítségével Formon meg is tudunk jeleníteni.

## Elkészült Windows Forms grafikus felület

Windows Formsban grafikus felületet készítettem, hogy könnyebben vizualizálhassuk a különböző DeCasteljau végrehajtási módszereket. A felületen egy ComboBox segítségével választhatjuk ki a kívánt módszert (pl. iteratív, párhuzamos, rekurzív), és az Execute gomb megnyomásával a kiválasztott algoritmus futtatásra kerül ugyanazon bemeneti kontrollpontokon. Két kontrollpont tömböt definiáltam: az egyik a szinuszgörbét reprezentál, míg a másik az előző tükörképét, amely az y-tengelyen még el lett tolva a hullám magasságával (amplitúdójával).

A grafikus felület lehetővé teszi a felhasználó számára, hogy összehasonlítsák a különböző módszerek teljesítményét és eredményeit.

Ez a megoldás különösen hasznos lehet azok számára, akik szeretnék megérteni a DeCasteljau algoritmus működését és hatékonyságát, miközben vizuálisan is érzékeltetjük az egyes végrehajtási módok közötti időbeli különbségeket.

![decasteljau-windowsforms-app](image.png)

## Párhuzamosítási módszerek összehasonlítása

Egy konzolos alkalmazást készítettem, hogy pontosabb képet kaphassunk a különböző algoritmusok futási idejéről a grafikus felület méréséhez képest. Ez az alkalmazás a grafikus felület által is használt DLL állományban található DeCasteljau-algoritmusokat futtatja.

Tesztjeim során 10 egymást követő futtatás átlagos futási idejét mértem, hogy csökkentsem az egyes futtatások közötti ingadozások hatását, amely ingadozások természetesek a rendszer teljesítménye, a háttérfolyamatok vagy a memóriakezelés eltérései miatt. Ügyeltem rá, hogy Release típusú buildet használjak, mivel ez a build fordítói optimalizálásokat végez, amelyek jobb teljesítményt adnak a kód futását tekintve, ellentétben a Debug builddel. A következő konzolos kimeneten látható egyik tesztem eredménye:

```bash
DeCasteljau Execution Times (ms) - Average of 10 consequent executions:
Number of controlPoints: 200, increment = 0,001
--------------------------------------------------------
Strategy                            Execution Time (ms)
--------------------------------------------------------
Iterative Single DeCasteljau          158,4101
Iterative Parallel DeCasteljau         39,2081
Iterative TPL DeCasteljau              40,0885
Recursive Parallel DeCasteljau         45,0664
--------------------------------------------------------
Speedup with Parallel.For: 4,04
Speedup with TPL: 3,95
Speedup with Recursive + TPL: 3,52
```

A különböző végrehajtási módok 200 kontrollponttal lettek tesztelve, és a t paraméter lépésköze 0.001 volt (ez 1000 DeCasteljau-iterációt jelent, grafikusan finomabb felbontású görbéket eredményezve), hogy biztosítsunk mérhető nagyságrendű számítási feladatot, miközben a párhuzamosítási stratégiák hatását pontosabban mérhetjük (ezen a szinten a szálkezelés overheadjei elhanyagolhatóak).

A következő eredmények születtek a különböző párhuzamosítási technikák alkalmazása során:

- Parallel.For alkalmazása esetén a teljesítmény 4.04-szeresére növekedett a szekvenciális futáshoz képest ebben a tesztben.

- TPL (_Task Parallel Library_) alkalmazása 3.95-szörös gyorsulást eredményezett, ami azt jelenti, hogy a TPL képes jobb párhuzamos végrehajtást biztosítani, mint a Parallel.For, különösen a dinamikus feladatkezelés és a finomabb vezérlés miatt. Volt olyan futás, amikor a TPL megelőzte az előző Parallel.For módszert is.

- A rekurzív DeCasteljau párhuzamosítása TPL-lel csupán 3.52-szeres gyorsulást eredményezett, ami arra utal, hogy a rekurzív hívások overheadje csökkenti a párhuzamosítás hatékonyságát.

### Összegzés:

Összességében a Parallel.For és a TPL módszerek biztosították a legnagyobb gyorsulást, és bár mindkét megoldás jól teljesített, a TPL jobb finomhangolást és rugalmasságot kínál, míg a Parallel.For egyszerűbb implementációval és az optimális szálkezelésével előnyösebb választásnak tűnt. A rekurzív DeCasteljau párhuzamosított változata, bár szintén jelentős javulást jelent a szekvenciális futtatáshoz képest, de elmarad a többi módszer hatékonyságától.

Ha a DeCasteljau algoritmus viszonylag kevés kontrollponttal dolgozik, a párhuzamosítás nem jár jelentős teljesítménybeli haszonnal, ilyenkor a szálkezelés overheadje még túl nagy. Ellenben ha olyan helyzetekről van szó, ahol sok kontrollpontot kell folyamatosan módosítani vagy újraszámolni (mint a grafikus szerkesztőalkalmazásokban), akkor mindenképpen előnyös a párhuzamosítás.

Szerkesztőprogramokban, ahol a felhasználó folyamatosan módosítja a görbéket, a DeCasteljau algoritmus minden egyes módosításkor újra végrehajtódik. Ilyenkor a párhuzamosítás segíthet abban, hogy a nagy számú kontrollpontot gyorsabban dolgozhassák fel, különösen komplex görbék vagy magasabb dimenziójú felületek esetén.

Ezért érdemes a közeljövőben alkalmazáslogikát építeni arra, hogy dinamikusan válasszuk ki, mikor hívjuk meg a párhuzamosított DeCasteljau algoritmust, figyelembe véve a kontrollpontok számát és a görbe finomításának mértékét. Egy olyan határértéket érdemes meghatározni, amely felett (például amikor legalább még nem ront a végrehajtási időn, 1-szeres sebességnövekedés esetén) már indokolt a párhuzamosítás alkalmazása, az alatt még a szekvenciális módszert választjuk.
