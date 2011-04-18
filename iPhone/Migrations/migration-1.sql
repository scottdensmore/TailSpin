CREATE TABLE surveys(id INTEGER PRIMARY KEY AUTOINCREMENT, title VARCHAR(100), tenant VARCHAR(100), imageUrl VARCHAR(500), durationInMinutes INTEGER, description VARCHAR(500));

CREATE TABLE questions(id INTEGER PRIMARY KEY, survey_id INTEGER, questionType VARCHAR(100), text VARCHAR(1000), possibleAnswers VARCHAR(1000));
