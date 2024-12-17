INSERT INTO encounters.encounters ("Id", "Name", "Description", "XP", "Status", "Type", "AuthorId", "Location_Latitude", "Location_Longitude", "IsReviewed", "PublishedDate", "ArchivedDate")
VALUES 
(15, 'Old Encounter', 'Old Description', 50, 'ACTIVE', 'SOCIAL', 1, 45.1234, 19.5678, false, NOW(), NOW());


INSERT INTO encounters.tourist_profiles
("Id", "Username", "Password", "Role", "IsActive", "XP", "CompletedEncountersIds")
VALUES
(-1, 'testuser', 'password123', 'Tourist', true, 100, '{1, 2}'), -- testuser je završio encountere 1 i 2
(-2, 'adventurer', 'adventure', 'Tourist', true, 150, '{3, 4, 5}'), -- adventurer je završio encountere 3, 4, 5
(-3, 'explorer', 'exploremore', 'Tourist', true, 200, '{6}'); -- explorer je završio encounter 6


SELECT * FROM encounters.encounters;