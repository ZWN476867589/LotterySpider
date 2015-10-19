﻿CREATE TABLE `LotterySerialNo` (
	`RowID`	INTEGER NOT NULL DEFAULT 1 PRIMARY KEY AUTOINCREMENT UNIQUE,
	`SerialID`	TEXT NOT NULL,
	`LotteryTypeID`	INTEGER NOT NULL,
	`OpenTime`	TEXT NOT NULL
);

CREATE TABLE `LotteryOriginData` (
	`RowID`	INTEGER NOT NULL DEFAULT 1 PRIMARY KEY AUTOINCREMENT UNIQUE,
	`LotteryTypeID`	INTEGER NOT NULL,
	`OriginData`	TEXT NOT NULL DEFAULT "",
	`DataUrl`	TEXT NOT NULL,
	`SerialNo`	TEXT NOT NULL,
	`Time`	TEXT NOT NULL
);

CREATE TABLE `LotteryBasicInfo` (
	`LotteryID`	INTEGER NOT NULL DEFAULT 1 PRIMARY KEY AUTOINCREMENT UNIQUE,
	`LotteryTypeID`	INTEGER NOT NULL,
	`LotteryName`	TEXT NOT NULL,
	`LotteryShortCode`	TEXT NOT NULL,
	`OpenTimeOfWeek`	TEXT NOT NULL,
	`StartSellTime`	TEXT NOT NULL DEFAULT "",
	`StartSerialNo`	TEXT NOT NULL,
	`SerialNoType`	TEXT NOT NULL,
	`SerialNoStartIndex`	INTEGER NOT NULL
);