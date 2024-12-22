-- 2_verify_tourist_profiles.sql
SELECT 
    "Id",
    "Username",
    "XP",
    ("XP" / 10) AS "Level",
    "CompletedEncountersIds"
FROM encounters.tourist_profile;
