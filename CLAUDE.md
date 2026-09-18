# HSK More Balance: Events

Incident tuning for the Hardcore SK modlist: tech-level gates for incidents, creepjoiner variants,
Raid Extension factions and Hospitality guests.
Harmony patches only, plus one settings def — safe to add to or remove from a save.

Split out of [HSKMoreHardcore](../HSKMoreHardcore) — that mod no longer carries these patches.

## Settings def

`Defs/Misc/EventSettings.xml` (`EventSettingsDef`, defName `HSKMoreBalanceEvents_Settings`).
Every patch falls back to its in-code defaults when the def is missing.

- `incidentMinTechLevel` — incident defName -> minimum player tech level
- `creepJoinerFormMinTechLevel` — `CreepJoinerFormKindDef` defName -> minimum player tech level
- `guestMaxTechAhead` / `guestMaxTechBehind` — how far a guest faction may sit above / below the
  player's tech level (-1 = no limit)

Player tech level comes from Ignorance Is Bliss (`IgnoranceCompat`, reflection, no DLL reference);
without it, the player faction's tech level is used.

## Patches

| File | What it does |
|---|---|
| `IncidentTechGate.cs` | Postfix on `IncidentWorker.CanFireNow` — gated incidents never fire (storyteller and dev menu alike) |
| `CreepJoinerTechGate.cs` | Prefixes `CreepJoinerUtility.GetCreepjoinerSpecifics` / `GenerateAndSpawn` — picks a random allowed form by weight |
| `RaidExtensionTechFilter.cs` | Postfix on Raid Extension's own `FactionCanBeGroupSource`, plus a prefix clearing a preset `parms.faction` for hostile caravan / traveler |
| `GuestTechFilter.cs` | Filters Hospitality visit planning (`PlanNewVisit`, `FillIncidentQueue`) and the storyteller path (`IncidentWorker_VisitorGroup.FactionCanBeGroupSource`); comms-console invites are exempt |

Each patch class is its own `[StaticConstructorOnStartup]` and applies its own Harmony patches —
there is no central init. All of them no-op when the mod they hook into is absent.

`RaidExtensionTechFilter` and `GuestTechFilter` have a `DebugLog` const at the top (currently `true`)
that logs every filter decision and every actual arrival.

## Build

```bash
dotnet build Source/HSKMoreBalanceEvents/HSKMoreBalanceEvents.v16.csproj -c Release
```

1.6 only. Output: `1.6/Assemblies/HSKMoreBalanceEvents.dll`. The csproj defaults `RimWorldPath`
to the Steam install; override with `-p:RimWorldPath=...`.

`create_junctions.ps1` links the folder into the RimWorld `Mods` directory.
