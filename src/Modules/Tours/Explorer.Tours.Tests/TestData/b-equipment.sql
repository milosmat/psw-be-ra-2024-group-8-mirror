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
	"Id", "Difficulty", "WalkRating", "BikeRating", "CarRating", "BoatRating", "InterestTags")
VALUES (-1, 1, 3, 2, 0, 1, '{"nature"}');

INSERT INTO tours."TourPreferences"(
	"Id", "Difficulty", "WalkRating", "BikeRating", "CarRating", "BoatRating", "InterestTags")
VALUES (-2, 2, 1, 1, 0, 1, '{"city"}');

INSERT INTO tours."TourPreferences"(
	"Id", "Difficulty", "WalkRating", "BikeRating", "CarRating", "BoatRating", "InterestTags")
VALUES (-3, 1, 3, 0, 0, 3, '{"beach"}');