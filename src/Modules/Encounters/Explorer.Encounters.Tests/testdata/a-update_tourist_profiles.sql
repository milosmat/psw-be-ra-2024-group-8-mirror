-- 4_update_tourist_profiles.sql
UPDATE encounters.tourist_profile
SET "XP" = "XP" + 50, "CompletedEncountersIds" = array_append("CompletedEncountersIds", '11')
WHERE "Username" = 'testuser';

UPDATE encounters.tourist_profile
SET "XP" = "XP" - 30, "CompletedEncountersIds" = array_remove("CompletedEncountersIds", '5')
WHERE "Username" = 'adventurer';
