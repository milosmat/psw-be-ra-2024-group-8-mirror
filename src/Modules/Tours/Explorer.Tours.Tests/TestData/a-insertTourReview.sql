/*INSERT INTO tours."TourReviews" (TourId, PersonId, Rating, Comment, TourDate, ReviewDate)
VALUES 
(1, 1, 5, 'Sjajna tura!', GETDATE() - 7, GETDATE()); -- Datum ture je 7 dana unazad*/
INSERT INTO tours."TourReviews"(
	"Id", "Rating", "Comment", "PersonnId", "TourDate", "ReviewDate", "Images", "TourId")
	VALUES (-1, 5, 'Sjajna tura!', -22,'2024-10-14 15:23:44.223+02', '2024-10-21 15:23:44.223+02', '{{"image.png"}}', -3);