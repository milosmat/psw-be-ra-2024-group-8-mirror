-- 3_additional_tourist_profiles.sql
INSERT INTO encounters.tourist_profile 
("Id", "Username", "Password", "Role", "IsActive", "XP", "CompletedEncountersIds")
VALUES
(-4, 'newbie', 'newbiepass', 'Tourist', true, 10, '{}'), -- Novi turist sa minimalnim XP
(-5, 'poweruser', 'strongpass', 'Tourist', true, 500, '{7, 8,9, 10}'); -- Napredni turist sa mnogo završenih encountera
