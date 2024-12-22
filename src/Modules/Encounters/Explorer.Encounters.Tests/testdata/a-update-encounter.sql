-- Dodavanje Encounter za test ažuriranja
INSERT INTO encounters.encounters 
    ("Id", "Name", "Description", "XP", "Status", "Type", "AuthorId", "Location_Latitude", "Location_Longitude", "IsReviewed", "PublishedDate", "ArchivedDate")
VALUES 
    (-3, 'Old Encounter', 'Old Description', 50, 'DRAFT', 'SOCIAL', 1, 45.1234, 19.5678, false, NOW(), NOW());
