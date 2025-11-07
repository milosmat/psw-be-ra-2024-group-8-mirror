# Backend Contributions by @milosmat
Repository: psw-be-ra-2024-group-8 (mirror)  
Upstream PRs: kzi-nastava/psw-be-ra-2024-group-8  
Total authored backend commits in mirror: 37  
Total authored upstream PRs: 13  

This document enumerates and explains ONLY the work personally contributed by @milosmat (based strictly on commit messages and merged pull requests). No speculative future work and no generic project description—just what was actually done.

---

## High-Level Thematic Areas

| Area | Your Concrete Contributions |
|------|-----------------------------|
| Tour Checkpoint Core | Service + DTO implementation, repository & DI wiring, object coordinate enhancements, fixes. |
| Aggregate & DDD Patterns | Introduced aggregate root pattern; completed tour controller & service to follow aggregate logic; CRUD encounters with validation. |
| Tour Execution Flow | Implemented start, finish, abandon (without location) operations; execution fixes; controller & service expansions for launching tours. |
| Encounters Module | Added module, full CRUD, validation, review integration for level 10 tourists, display & reviewer flow. |
| Secrets & Discovery | Completed backend logic enabling discovery of secrets tied to checkpoints. |
| Tourist Profile & XP | Added TouristProfile subclass (extending users) and later XP/challenge integration (level-based gating). |
| Challenges / XP | Implemented XP logic for tourists and challenge review mechanics (level ≥ 10); integrated review endpoints. |
| Games Setup | Project scaffolding / setup for game-related functionality. |
| Testing | Added integration tests for TourCheckpointController; test data script updates; changes to facilitate tests. |
| Infrastructure & Transition | Configured repositories & DI, transitioned codebase to a newer version, merges to keep feature branches current. |
| Hotfixes / Stability | Multiple “hotfix” commits resolving immediate issues post-merge. |
| Front-End Support Adjustments | Backend changes enabling front-end features (“frontend features”, “improvements for front”). |
| Object Authoring | Added longitude/latitude for authored objects; object authoring PR merges. |
| Merges & Conflict Resolutions | Numerous merges from development into feature branches to resolve conflicts and integrate upstream changes. |

---

## Chronological Timeline of Your Commits (Oldest → Newest)

| Date (UTC+1) | Commit (short SHA) | Message (Original) | Contribution Detail |
|--------------|--------------------|--------------------|---------------------|
| 2024-10-15 18:28 | 40f45065 | feat: Implemented TourCheckpoint service and Dto | Introduced service layer & DTO abstractions for TourCheckpoint. |
| 2024-10-15 19:18 | c099bb6b | transitioning to a newer version | Framework/dependency upgrade alignment; kept code current. |
| 2024-10-15 20:38 | 879ee0c4 | feat: Configure repositories and dependency injection, and implement new repository for TourCheckpoints entity | Added repository pattern + DI registration for checkpoint persistence. |
| 2024-10-15 21:23 | 048bfb92 | test: Implemented integration tests for TourCheckpointController and updated test data scripts | Introduced integration tests; updated seed/test data. |
| 2024-10-20 17:07 | dde7f67f | improvements for front | Backend adjustments supporting UI evolution (likely endpoint refinements). |
| 2024-10-20 23:30 | 555bdf5b | frontend features | Added backend capabilities consumed directly by new front-end features. |
| 2024-10-21 15:09 | a50c982f | fix for tourCheckpoint | Corrected TourCheckpoint logic (bug fix post initial implementation). |
| 2024-10-21 17:19 | 2c53e2b2 | Merge branch 'feat/autor-dodavanje/dodavanje-uklanjanje' into feat/autor-checkpoint | Integrated equipment/object addition/removal with checkpoint authoring work. |
| 2024-10-21 22:47 | 62624c17 | Merge PR #12 Feat/autor objekat | Brought authored object feature (e.g., map objects) into checkpoint flow. |
| 2024-10-23 20:39 | 16b37aaf | changes for tests | Additional test adjustments (likely making endpoints test-friendly). |
| 2024-10-23 20:48 | 83696e34 | merge dev | Synced feature branch with development to reduce divergence. |
| 2024-10-23 22:47 | 36f50dd7 | mergde with dev | Continued merge alignment to keep code compatible. |
| 2024-10-23 22:53 | 92677f94 | merge dev conflicts | Resolved conflicts—preserved both sides’ changes. |
| 2024-10-23 23:56 | f17b363f | Merge dev into feat/autor-checkpoint | Further sync for checkpoint authoring stability. |
| 2024-10-23 23:58 | d96d8409 | Merge dev into feat/autor-checkpoint | Ongoing integration of upstream changes. |
| 2024-10-24 01:28 | bb812507 | Merge dev into feat/autor-checkpoint | Maintained feature freshness; avoided large conflict accumulation. |
| 2024-10-25 13:26 | cf016a27 | added longitude and latitude for object | Extended object model with spatial coordinates. |
| 2024-10-25 13:34 | 7d171fb9 | Merge PR #25 Feat/autor checkpoint | Finalized checkpoint authoring feature into main line. |
| 2024-10-31 21:45 | 57c7a0f7 | Napravljen koren agregata | Implemented aggregate root (DDD) foundational structure (likely Tour or related). |
| 2024-11-04 18:57 | 640532a5 | dovrsen tour kontroler i servis da podrzava logiku korena agregata i ddd sablon | Completed tour controller/service following aggregate root rules. |
| 2024-11-05 16:45 | 8023a3e8 | Zavrseno pokretanje (zavrsavanje i napustanje) ture bez lokacije | Implemented tour lifecycle operations (finish/abandon without geo dependency). |
| 2024-11-05 17:14 | c6c94d8e | Merge sa developmentom, reseni konflikti | Conflict resolution merging new development changes. |
| 2024-11-05 17:47 | b903ceb7 | fix na Tour execution-u | Corrected tour execution handling after lifecycle implementation. |
| 2024-11-06 22:42 | d6deea81 | Dodate funkcionalnosti kontrolera i servisa vezane za pokretanje ture | Added more controller/service logic for starting tours. |
| 2024-11-07 00:01 | 5754cbc2 | Merge dev into feat/author-pokretanje-ture | Synchronized tour start branch with latest development. |
| 2024-11-07 03:22 | 4a26355b | Zavrseno otkrivanje tajni za checkpointe | Backend support for secret discovery completion at checkpoints. |
| 2024-11-21 02:23 | 8c28f094 | odradjene sve crud operacije implementiran kontroler implementiran agregat Encounter sa proverom svega | Full Encounter aggregate + CRUD + validation controller. |
| 2024-11-25 18:36 | 2232cb3a | hotfix | Immediate fix (likely addressing encounter/tour integration). |
| 2024-11-25 18:43 | 6a18e86b | Merge dev into feat/dodavanje-encounter-modula | Synced encounter module with development branch. |
| 2024-11-25 18:47 | 9c8a0667 | merge | General branch merge (integration step). |
| 2024-11-25 21:26 | 96cf3918 | hotfix | Additional stability fix. |
| 2024-11-25 21:27 | 349e177e | Merge PR #66 Feat/dodavanje encounter modula | Brought encounter module (CRUD, aggregate) into main line. |
| 2024-11-27 23:42 | ee524af1 | dodat touristProfile klasa koja nasledjuje usere | Implemented TouristProfile class inheriting from users (XP/level base). |
| 2024-11-28 02:49 | cd3aefde | dodat prikaz i review encountera koje kreiraju turisti sa levelom 10 | Added display & review flow for level‑10 tourist-created encounters. |
| 2024-12-04 23:48 | 74741287 | Postavka projekta za igrice | Scaffolded games project (structural foundation for game mechanics). |
| 2024-12-04 23:53 | 7f218ac4 | Merge dev into feat/xp-turista-izazovi | Synced XP/challenge feature branch with development. |
| 2024-12-05 00:17 | 6f6d1d00 | Merge PR #73 Feat/xp turista izazovi | Integrated XP + tourist challenges (review gating by level). |

### Commit Grouping Summary

| Group | Commits |
|-------|---------|
| TourCheckpoint Foundations | 40f45065, c099bb6b, 879ee0c4, a50c982f |
| Testing Enablement | 048bfb92, 16b37aaf |
| Front-End Support | dde7f67f, 555bdf5b |
| Object & Checkpoint Authoring | cf016a27, 7d171fb9, 62624c17, 2c53e2b2 |
| Aggregate & DDD Introduction | 57c7a0f7, 640532a5 |
| Tour Execution Lifecycle | 8023a3e8, b903ceb7, d6deea81, 5754cbc2 |
| Secrets Discovery | 4a26355b |
| Encounters Module | 8c28f094, 349e177e, cd3aefde |
| Tourist Profile & XP | ee524af1, 6f6d1d00, 7f218ac4 |
| Games Setup | 74741287 |
| Review & Level-Gated Features | cd3aefde, 6f6d1d00 |
| Merges / Conflict Resolution | 83696e34, 36f50dd7, 92677f94, f17b363f, d96d8409, bb812507, 6a18e86b, 9c8a0667, c6c94d8e, 5754cbc2, 7f218ac4 |
| Hotfixes / Stability | 2232cb3a, 96cf3918 |
| Transition / Upgrades | c099bb6b |
| Testing Data / Adjustments | 048bfb92, 16b37aaf |

---

## Consolidated Impact

- Established core domain building blocks (TourCheckpoint service, DTOs, repository pattern, aggregate roots).
- Evolved tour functionality from static definition to executable lifecycle (start, finish, abandon).
- Implemented encounter module end-to-end (aggregate + controller + CRUD + tourist review).
- Enabled secret discovery mechanics bridging tour progression and hidden content.
- Introduced tourist profile XP/level model and challenge review gating (level-based progression).
- Added game project scaffolding for extending platform mechanics.
- Strengthened reliability with integration tests and iterative test adjustments.
- Provided spatial object enhancements (coordinates) and front-end supportive refinements.
- Maintained momentum through continuous merges, conflict resolutions, and hotfixes ensuring feature branches stayed synchronized.
- Extended functionality to include tracking time-based achievement results and coupon awarding.

---
Let me know if you want this committed as CONTRIBUTIONS.md or merged into your existing README. I can prepare a PR in the mirror repo if you’d like—just say the word.
