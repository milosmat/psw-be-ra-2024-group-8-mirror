INSERT INTO encounters.encounters ("Id", "Name", "Description", "XP", "Status", "Type", "AuthorId", "Location_Latitude", "Location_Longitude", "IsReviewed", "PublishedDate", "ArchivedDate")
VALUES 
(-1, 'Old Encounter', 'Old Description', 50, 'ACTIVE', 'SOCIAL', 1, 45.1234, 19.5678, false, NOW(), NOW()),
(25, 'Old Encounter', 'Old Description', 50, 'DRAFT', 'SOCIAL', 1, 45.1234, 19.5678, false, NOW(), NOW()),
(-14, 'Old Encounter', 'Old Description', 50, 'ACTIVE', 'SOCIAL', 1, 45.1234, 19.5678, false, NOW(), NOW());
