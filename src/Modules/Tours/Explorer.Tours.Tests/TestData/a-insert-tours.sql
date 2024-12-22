INSERT INTO tours."Tours" (
    "Id", "Name", "Description", "Weight", "Tags", "Status", "Price", "LengthInKm", "PublishedDate", 
    "ArchivedDate", "TravelTimes", "DailyAgendas", "AuthorId"
)
VALUES (
    1, 
    'Tura1', 
    'Lepa tura', 
    'medium', 
    'prvi, drugi',
    1, 
    200, 
    15, 
    '2024-12-05T10:38:07.011769+01:00', 
    '2024-12-05T10:38:07.011769+01:00', 
    '[{"Time": 100, "TransportType": 1}]',
    '[{"Day": 2, "Description": "opis1", "EndDestination": "drugi", "StartDestination": "prvi", "BetweenDestinations": ["dest1", "dest2"]}]', 
    1
);
