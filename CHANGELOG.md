# Change Log
All notable changes to this project will be documented in this file.
 
The format is based on [Keep a Changelog](http://keepachangelog.com/)
and this project adheres to [Semantic Versioning](http://semver.org/).

## 0.10.0-alpha (2024-04-11)

### Changed

Now, when checking the conditions for imposing an effect, the actor and the battlefield are passed in so that it can be used when checking the conditions.

## 0.9.0-alpha (2024-03-30)

### Changed

Status lifetime UntilCombatantEffectMeetPredicatesLifetime can detect combatant change his state like HP or position. This is needed to enter statuses like Last Breath. The idea is for the status to last as long as the combatant's health is above a threshold. And then the status disappears.

## 0.8.0-alpha (2024-03-26)

### Changed

Aura can now select targets based on position on the field.

For this purpose, the signature of IAuraTargetSelector was changed, which must be taken into account in ready-made implementations.

## 0.7.1-alpha (2024-03-26)

### Fixed

The design of the aura has been corrected so that the owner of the aura can be accessed at the time of constructing the imposed statuses. This is a very important thing for an aura - the lifetime of effects on targets should depend on the lifetime of the owner of the aura.

## 0.7.0-alpha (2024-03-25)

### Added

Add status - Aura.

The main idea behind status is to impose other statuses on combatants other than the owner of the aura.

A new lifetime has also been added for the aura. Unlike the existing OwnerBounded, which lives as long as the owner of the status is alive, TargetCombatantsOwner lives on the owner of the status as long as another specified combatant (or multiple combatants) is alive.

Aura is a very popular and cool tool in modern RPGs.

## 0.6.1-alpha (2024-03-12)

### Fixed

Fixed the lifetime lasting for multiple combatant turns.

## 0.6.0-alpha (2024-01-25)

### Added

This version adds the ability to apply modifiers to CombatMovement. This is necessary so that during battle statuses change the damage or strength of buffs.

Metadata has also been added for use in statuses. Now you can make statuses that, for example, only enhance melee CombatMovement.

## 0.5.0-alpha (2024-01-12)

### Added

This version adds the ability to track the reasons for changes in any characteristic. For example, to see which status increased the damage of a particular CombatMovement. This is very important for the UI in an RPG, where there can be many characteristics, equipment, various effects and statuses that affect the combatant.

## 0.4.0-alpha (2023-12-13)

### Added

The concept of reason for displacement was introduced. This can be important during visualization, when you need to know why the combatant changed position - he moved or someone pushed him.

## 0.3.0-alpha (2023-11-14)

### Added

The combat state is placed in a separate abstraction. Also, the method for calculating the battle status has been moved outside the framework.

In specific implementations of turn-based RPGs, there may be different reasons for winning and losing battles. There may also be additional statuses other than Victory and Defeat.

All these points can now be realized independently. Or use the built-in basic implementations of the framework.

## 0.1.0-alpha (2023-07-25)
  
Basic combat engine with maneuvers and two-side battlefield like in a jRPGs.

Features:
*   Combat rounds when all of combatants complete turns.
*   Damage, stun, add status, change combat movement damage effects.
*   Two-side battle field to fight wall-by-wall.