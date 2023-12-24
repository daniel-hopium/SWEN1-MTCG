CREATE DATABASE trading_card_game_db;

\c trading_card_game_db;


CREATE EXTENSION IF NOT EXISTS "uuid-ossp";

CREATE TABLE IF NOT EXISTS users (
    id              uuid            DEFAULT uuid_generate_v4() PRIMARY KEY ,
    username        VARCHAR(255)    NOT NULL UNIQUE,
    password        VARCHAR(255)    NOT NULL,
    name            VARCHAR(50)     ,
    image           VARCHAR(50)     ,
    bio             TEXT            ,
    coins           INTEGER         NOT NULL DEFAULT 20,
    created         TIMESTAMP       NOT NULL DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE IF NOT EXISTS cards (
    id              uuid            PRIMARY KEY,
    name            VARCHAR(255)    NOT NULL,
    damage          INT             NOT NULL,
    element_type    VARCHAR(255)   ,
    card_type       VARCHAR(255)    ,
    created         TIMESTAMP       NOT NULL DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE IF NOT EXISTS packages (
    id              uuid            PRIMARY KEY,
    name            VARCHAR(255)    ,
    price           INT             NOT NULL DEFAULT 5,
    created         TIMESTAMP       NOT NULL DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE IF NOT EXISTS package_cards
(
    id              uuid            DEFAULT uuid_generate_v4() PRIMARY KEY,
    package_id      uuid            NOT NULL REFERENCES packages (id) ON DELETE CASCADE,
    card_id         uuid            NOT NULL REFERENCES cards (id) ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS user_cards
(
    id              uuid            DEFAULT uuid_generate_v4() PRIMARY KEY,
    user_id         uuid            NOT NULL REFERENCES users (id) ON DELETE CASCADE,
    card_id         uuid            NOT NULL REFERENCES cards (id) ON DELETE CASCADE,
    is_in_deck      bool            NOT NULL DEFAULT FALSE,
    usage           varchar(255)    NOT NULL DEFAULT 'none' 
);

CREATE TABLE IF NOT EXISTS battles
(
    id              uuid            DEFAULT uuid_generate_v4() PRIMARY KEY,
    timestamp       TIMESTAMP       NOT NULL DEFAULT CURRENT_TIMESTAMP,
    winner_id       uuid            NOT NULL REFERENCES users (id),
    opponent_id     uuid            NOT NULL REFERENCES users (id),
    log             TEXT            NOT NULL
);

CREATE TABLE IF NOT EXISTS scoreboard
(
    id              uuid            PRIMARY KEY,
    user_id         uuid            NOT NULL REFERENCES users (id) ON DELETE CASCADE,
    elo             INTEGER         NOT NULL DEFAULT 1000,
    wins            INTEGER         NOT NULL DEFAULT 0,
    losses          INTEGER         NOT NULL DEFAULT 0
);


CREATE TABLE IF NOT EXISTS trades
(
    id              uuid            PRIMARY KEY,
    created         TIMESTAMP       NOT NULL DEFAULT CURRENT_TIMESTAMP,
    user_card_id    uuid            NOT NULL REFERENCES user_cards (id) ON DELETE CASCADE,
    minimum_damage  INTEGER         NOT NULL,
    price           INTEGER         NOT NULL DEFAULT 1
);