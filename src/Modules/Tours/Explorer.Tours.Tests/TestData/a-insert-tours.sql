/*INSERT INTO tours.Tours (Name, Description, Weight, Tags, Status, Price) VALUES
('Tura po prirodi', 'Uživajte u prirodnim lepotama.', '10kg', ARRAY['priroda', 'avantura'], 'DRAFT', 150.00),
('Kulturna tura', 'Istražite istorijske znamenitosti.', '5kg', ARRAY['kultura', 'istorija'], 'PUBLISHED', 200.00),
('Gastro tura', 'Uživajte u lokalnoj kuhinji.', '3kg', ARRAY['hrana', 'kultura'], 'DRAFT', 100.00);
*/

INSERT INTO tours."Tours"(
	"Id", "Name", "Description", "Weight", "Tags", "Status", "Price", "LengthInKm", "PublishedDate", "ArchivedDate", "TravelTimes", "DailyAgendas", "AuthorId")
	VALUES (-1, 'Name1', 'Description1', 'Weight1', '{{"Tags1"}}', 0, 255, 1500, '2024-10-14 15:23:44.223+02', '2024-10-24 15:23:44.223+02', '[{{}}]', '[{{}}]', -11);

INSERT INTO tours."Tours"(
	"Id", "Name", "Description", "Weight", "Tags", "Status", "Price", "LengthInKm", "PublishedDate", "ArchivedDate", "TravelTimes", "DailyAgendas", "AuthorId")
	VALUES (-2, 'Name2', 'Description2', 'Weight2', '{{"Tags2"}}', 0, 400, 2000, '2024-10-14 15:23:44.223+02', '2024-10-24 15:23:44.223+02', '[{{}}]', '[{{}}]', -12);

INSERT INTO tours."Tours"(
	"Id", "Name", "Description", "Weight", "Tags", "Status", "Price", "LengthInKm", "PublishedDate", "ArchivedDate", "TravelTimes", "DailyAgendas", "AuthorId")
	VALUES (-3, 'Name3', 'Description3', 'Weight3', '{{"Tags3"}}', 0, 300, 3000, '2024-10-04 15:23:44.223+02', '2024-10-29 15:23:44.223+02', '[{{}}]', '[{{}}]', -13);
