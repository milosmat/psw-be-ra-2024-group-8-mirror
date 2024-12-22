INSERT INTO encounters.encounters ("Id", "Name", "Description", "XP", "Status", "Type", "AuthorId", "Location_Latitude", "Location_Longitude", "IsReviewed", "PublishedDate", "ArchivedDate")
VALUES 
(10, 'Encounter to Delete', 'This encounter will be deleted.', 50, 'ACTIVE', 'MISC', 2, 45.1234, 19.5678, false, NOW(), NOW());
