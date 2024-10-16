INSERT INTO tours."Equipment"(
    "Id", "Name", "Description")
VALUES (-1, 'Voda', 'Količina vode varira od temperature i trajanja ture. Preporuka je da se pije pola litre vode na jedan sat umerena fizičke aktivnosti (npr. hajk u prirodi bez značajnog uspona) po umerenoj vrućini');
INSERT INTO tours."Equipment"(
    "Id", "Name", "Description")
VALUES (-2, 'Štapovi za šetanje', 'Štapovi umanjuju umor nogu, pospešuju aktivnost gornjeg dela tela i pružaju stabilnost na neravnom terenu.');
INSERT INTO tours."Equipment"(
    "Id", "Name", "Description")
VALUES (-3, 'Obična baterijska lampa', 'Baterijska lampa od 200 do 400 lumena.');

INSERT INTO tours."TourPreferences"(
	"Id", "Difficulty", "TransportPreferences", "InterestTags")
VALUES (1, 2, to_jsonb('{"WALK": 2, "BIKE": 3, "CAR": 1}'), to_jsonb('["nature", "adventure", "culture"]'));
INSERT INTO tours."TourPreferences"(
	"Id", "Difficulty", "TransportPreferences", "InterestTags")
VALUES (2, 1, to_jsonb('{"WALK": 1, "BIKE": 0, "CAR": 2}'), to_jsonb('["adventure"]'));
INSERT INTO tours."TourPreferences"(
	"Id", "Difficulty", "TransportPreferences", "InterestTags")
VALUES (3, 2, to_jsonb('{"WALK": 1, "BIKE": 1, "CAR": 1}'), to_jsonb('["culture"]'));