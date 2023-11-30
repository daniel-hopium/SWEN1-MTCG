CREATE DATABASE trading_card_game_db;

\c trading_card_game_db;

CREATE TABLE IF NOT EXISTS users (
    id          SERIAL          PRIMARY KEY ,
    username    VARCHAR(255)    NOT NULL UNIQUE,
    password    VARCHAR(255)    NOT NULL,
    name        VARCHAR(50)     ,
    image       Varchar(50)     ,
    bio         TEXT            
    
);

INSERT INTO users (username, password, name, image) VALUES
    ('user1', 'password1', 'John Doe', ':=)'),
    ('user2', 'password2', 'Jane Smith', ':=)'),
    ('user3', 'password3', 'Bob Johnson', ':=)'),
    ('user4', 'password4', 'Alice Brown', ':=)'),
    ('user5', 'password5', 'Charlie White', ':=)');

