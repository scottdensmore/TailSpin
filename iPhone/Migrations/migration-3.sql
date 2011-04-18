CREATE TABLE survey_responses(
    id INTEGER PRIMARY KEY AUTOINCREMENT, 
    surveyId INTEGER NOT NULL,
    isFavorite BOOL,
    progressPercentage FLOAT,
    readyForSubmission BOOL,
    lastSyncDate DATETIME
);

CREATE TABLE survey_response_answers(
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    surveyResponse_id INTEGER NOT NULL,
    localFileUrl VARCHAR(200),
    answer VARCHAR(500),
    questionIndex INTEGER
);