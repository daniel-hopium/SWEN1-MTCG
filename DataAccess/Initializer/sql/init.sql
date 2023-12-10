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
    losses      INTEGER         NOT NULL DEFAULT 0
);

CREATE TABLE cards (
    id     uuid            DEFAULT uuid_generate_v4() PRIMARY KEY,
    name        VARCHAR(255)    NOT NULL,
    damage      INT             NOT NULL,
    element_type VARCHAR(255)   NOT NULL,
    card_type   VARCHAR(255)    NOT NULL,
    user_id     uuid            REFERENCES users(id) ON DELETE CASCADE
);

CREATE TABLE packages (
    id          uuid            DEFAULT uuid_generate_v4() PRIMARY KEY,
    name        VARCHAR(255)    NOT NULL,
    price       INT             NOT NULL DEFAULT 5
);

CREATE TABLE package_cards
(
    id         uuid DEFAULT uuid_generate_v4() PRIMARY KEY,
    package_id uuid NOT NULL REFERENCES packages (id) ON DELETE CASCADE,
    card_id    uuid NOT NULL REFERENCES cards (id) ON DELETE CASCADE
);


CREATE TABLE user_cards
(
    id      uuid        DEFAULT uuid_generate_v4() PRIMARY KEY,
    user_id uuid        NOT NULL REFERENCES users (id) ON DELETE CASCADE,
    card_id uuid         NOT NULL REFERENCES cards (id) ON DELETE CASCADE
);

CREATE TABLE decks
(
    id      uuid DEFAULT uuid_generate_v4() PRIMARY KEY,
    name    VARCHAR(255) NOT NULL UNIQUE,
    user_id uuid      NOT NULL REFERENCES users (id) ON DELETE CASCADE
);

CREATE TABLE deck_cards
(
    id      uuid            DEFAULT uuid_generate_v4() PRIMARY KEY,
    deck_id uuid            NOT NULL REFERENCES decks (id) ON DELETE CASCADE,
    card_id uuid            NOT NULL REFERENCES cards (id) ON DELETE CASCADE
);

CREATE TABLE battles
(
    id      uuid            DEFAULT uuid_generate_v4() PRIMARY KEY,
    timestamp TIMESTAMP     NOT NULL DEFAULT CURRENT_TIMESTAMP,
    user_id uuid         NOT NULL REFERENCES users (id),
    deck_id uuid            NOT NULL REFERENCES decks (id),
    opponent_id uuid     NOT NULL REFERENCES users (id),
    opponent_deck_id uuid   NOT NULL REFERENCES decks (id),
    winner_id uuid       NOT NULL REFERENCES users (id),
    winner_deck_id uuid     NOT NULL REFERENCES decks (id)
);

CREATE TABLE scoreboard 
(
    id      uuid DEFAULT uuid_generate_v4() PRIMARY KEY,
    user_id uuid      NOT NULL REFERENCES users (id),
    elo     INTEGER      NOT NULL
);

CREATE TABLE trades
(
    id      uuid DEFAULT uuid_generate_v4() PRIMARY KEY,
    timestamp TIMESTAMP     NOT NULL DEFAULT CURRENT_TIMESTAMP,
    user_id uuid      NOT NULL REFERENCES users (id),
    card_id uuid         NOT NULL REFERENCES cards (id),
    price   INTEGER      NOT NULL
);

CREATE TABLE trade_offers
(
    id      uuid DEFAULT uuid_generate_v4() PRIMARY KEY,
    trade_id uuid        NOT NULL REFERENCES trades (id),
    user_id uuid      NOT NULL REFERENCES users (id),
    card_id uuid         NOT NULL REFERENCES cards (id)
);







INSERT INTO users (username, password, name, image) VALUES
    ('user1', 'password1', 'John Doe', ':=)'),
    ('user2', 'password2', 'Jane Smith', ':=)'),
    ('user3', 'password3', 'Bob Johnson', ':=)'),
    ('user4', 'password4', 'Alice Brown', ':=)'),
    ('user5', 'password5', 'Charlie White', ':=)');

