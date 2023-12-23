CREATE DATABASE trading_card_game_db;

\c trading_card_game_db;


CREATE EXTENSION IF NOT EXISTS "uuid-ossp";

CREATE TABLE users (
    id          uuid          DEFAULT uuid_generate_v4() PRIMARY KEY ,
    username    VARCHAR(255)    NOT NULL UNIQUE,
    password    VARCHAR(255)    NOT NULL,
    name        VARCHAR(50)     ,
    image       Varchar(50)     ,
    bio         TEXT            ,
    coins       INTEGER         NOT NULL DEFAULT 20,
    elo         INTEGER         NOT NULL DEFAULT 100,
    wins        INTEGER         NOT NULL DEFAULT 0,
    losses      INTEGER         NOT NULL DEFAULT 0,
    created   TIMESTAMP       NOT NULL DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE cards (
    id     uuid                 PRIMARY KEY,
    name        VARCHAR(255)    NOT NULL,
    damage      INT             NOT NULL,
    element_type VARCHAR(255)   ,
    card_type   VARCHAR(255)    ,
    user_id     uuid            REFERENCES users(id) ON DELETE CASCADE,
    created   TIMESTAMP       NOT NULL DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE packages (
    id          uuid            PRIMARY KEY,
    name        VARCHAR(255)    ,
    price       INT             NOT NULL DEFAULT 5,
    created   TIMESTAMP       NOT NULL DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE package_cards
(
    id         uuid DEFAULT uuid_generate_v4() PRIMARY KEY,
    package_id uuid NOT NULL REFERENCES packages (id) ON DELETE CASCADE,
    card_id    uuid NOT NULL REFERENCES cards (id) ON DELETE CASCADE
);

CREATE TABLE user_cards
(
    id              uuid        DEFAULT uuid_generate_v4() PRIMARY KEY,
    user_id         uuid        NOT NULL REFERENCES users (id) ON DELETE CASCADE,
    card_id         uuid        NOT NULL REFERENCES cards (id) ON DELETE CASCADE,
    is_in_deck      bool        NOT NULL DEFAULT FALSE,
    usage           varchar(255) NOT NULL DEFAULT 'NONE' 
);

CREATE TABLE battles
(
    id      uuid            DEFAULT uuid_generate_v4() PRIMARY KEY,
    timestamp TIMESTAMP     NOT NULL DEFAULT CURRENT_TIMESTAMP,
    user_id uuid         NOT NULL REFERENCES users (id),
    opponent_id uuid     NOT NULL REFERENCES users (id),
    winner_id uuid       NOT NULL REFERENCES users (id)
);

CREATE TABLE scoreboard 
(
    id      uuid DEFAULT uuid_generate_v4() PRIMARY KEY,
    user_id uuid      NOT NULL REFERENCES users (id),
    elo     INTEGER      NOT NULL
);


CREATE TABLE trades
(
    id          uuid            PRIMARY KEY,
    created   TIMESTAMP       NOT NULL DEFAULT CURRENT_TIMESTAMP,
    user_card_id uuid           NOT NULL REFERENCES user_cards (id) ON DELETE CASCADE,
    minimum_damage INTEGER      NOT NULL,
    price       INTEGER         NOT NULL DEFAULT 1
);







INSERT INTO users (username, password, name, image) VALUES
    ('user1', 'password1', 'John Doe', ':=)'),
    ('user2', 'password2', 'Jane Smith', ':=)'),
    ('user3', 'password3', 'Bob Johnson', ':=)'),
    ('user4', 'password4', 'Alice Brown', ':=)'),
    ('user5', 'password5', 'Charlie White', ':=)');

