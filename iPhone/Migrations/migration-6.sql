BEGIN TRANSACTION;
CREATE TABLE survey_responses_new(
    id INTEGER PRIMARY KEY AUTOINCREMENT, 
    surveyId INTEGER NOT NULL,
    progressPercentage FLOAT,
    readyForSubmission BOOL
);
INSERT INTO survey_responses_new (surveyId, progressPercentage, readyForSubmission) 
	SELECT surveyId, progressPercentage, readyForSubmission FROM survey_responses;
DROP TABLE survey_responses;
ALTER TABLE survey_responses_new RENAME TO survey_responses;
COMMIT;