export interface APIServer {
    Environment: Env;
    Logging: Logging;
    Player: Uri;
    Board: Uri;
    ScoreCard: Uri;
    Statistics: Uri;
    Leaderboard: Uri;
    AuditLog: Uri;
}

export interface Settings {
    apiServer: APIServer;
    environment: Env;
    logging: Logging;
}

export interface Uri {
    host: string;
    url: string;
}

export interface Env {
    name: string;
}

export interface Logging {
    console: boolean;
}