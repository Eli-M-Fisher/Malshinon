-- כאן עשיתי טבלת האנשים (מדווחים ויעדים)
CREATE TABLE people (
    id INT AUTO_INCREMENT PRIMARY KEY,
    full_name VARCHAR(255),
    secret_code VARCHAR(64) UNIQUE,
    created_at DATETIME DEFAULT CURRENT_TIMESTAMP
);

-- טבלת הזאת בשביל הדיווחים
CREATE TABLE reports (
    id INT AUTO_INCREMENT PRIMARY KEY,
    reporter_id INT,
    target_id INT,
    report_text TEXT,
    timestamp DATETIME,
    FOREIGN KEY (reporter_id) REFERENCES people(id),
    FOREIGN KEY (target_id) REFERENCES people(id)
);

-- וזו טבלה להתראות
CREATE TABLE alerts (
    id INT AUTO_INCREMENT PRIMARY KEY,
    target_id INT,
    alert_time_window_start DATETIME,
    alert_time_window_end DATETIME,
    reason TEXT,
    created_at DATETIME DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (target_id) REFERENCES people(id)
);

--people: כל אדם מזוהה בשם וקוד סודי (מדווח או יעד).


--reports: כל דיווח מקושר למדווח ויעד + טקסט + זמן.


--alerts: נשלפות כשיעד מזוהה כחשוד (פיקים או הופעה מרובה).



