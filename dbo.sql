/*
Navicat SQL Server Data Transfer

Source Server         : 120.92.227.117外网
Source Server Version : 105000
Source Host           : 120.92.227.117,1433:1433
Source Database       : mseed_platform
Source Schema         : dbo

Target Server Type    : SQL Server
Target Server Version : 105000
File Encoding         : 65001

Date: 2017-03-29 00:10:39
*/


-- ----------------------------
-- Table structure for game_access_auth
-- ----------------------------
DROP TABLE [dbo].[game_access_auth]
GO
CREATE TABLE [dbo].[game_access_auth] (
[api_access_id] bigint NOT NULL IDENTITY(10001,1) ,
[game_service_id] bigint NOT NULL ,
[access_level] int NOT NULL ,
[access_auth_key] nvarchar(128) NOT NULL ,
[reg_date] datetime NOT NULL 
)


GO

-- ----------------------------
-- Records of game_access_auth
-- ----------------------------
SET IDENTITY_INSERT [dbo].[game_access_auth] ON
GO
INSERT INTO [dbo].[game_access_auth] ([api_access_id], [game_service_id], [access_level], [access_auth_key], [reg_date]) VALUES (N'10001', N'1', N'100', N'test', N'2016-08-08 15:47:37.500')
GO
GO
SET IDENTITY_INSERT [dbo].[game_access_auth] OFF
GO

-- ----------------------------
-- Table structure for game_service
-- ----------------------------
DROP TABLE [dbo].[game_service]
GO
CREATE TABLE [dbo].[game_service] (
[game_service_id] bigint NOT NULL IDENTITY(1,1) ,
[service_name] nvarchar(80) NOT NULL ,
[ssl_certificate_path] nvarchar(256) NOT NULL ,
[push_certificate_path] nvarchar(256) NOT NULL ,
[reg_date] datetime NOT NULL 
)


GO

-- ----------------------------
-- Records of game_service
-- ----------------------------
SET IDENTITY_INSERT [dbo].[game_service] ON
GO
INSERT INTO [dbo].[game_service] ([game_service_id], [service_name], [ssl_certificate_path], [push_certificate_path], [reg_date]) VALUES (N'1', N'darkblaze_china_test', N'', N'', N'2016-08-08 15:47:37.500')
GO
GO
SET IDENTITY_INSERT [dbo].[game_service] OFF
GO

-- ----------------------------
-- Table structure for game_service_info
-- ----------------------------
DROP TABLE [dbo].[game_service_info]
GO
CREATE TABLE [dbo].[game_service_info] (
[game_service_id] bigint NOT NULL ,
[service_type] int NOT NULL ,
[service_status] tinyint NOT NULL ,
[service_app_id] nvarchar(256) NOT NULL ,
[service_secret] nvarchar(256) NOT NULL ,
[reg_date] datetime NOT NULL ,
[for_test] nvarchar(256) NULL 
)


GO

-- ----------------------------
-- Records of game_service_info
-- ----------------------------
INSERT INTO [dbo].[game_service_info] ([game_service_id], [service_type], [service_status], [service_app_id], [service_secret], [reg_date], [for_test]) VALUES (N'1', N'102', N'0', N'207264359517-3mlrqs1crlem39sgq439c20s2eomgov3.apps.googleusercontent.com', N'AIzaSyCTIw7p4VFCBS8HF7qgN2BqXV0cpnhhaVs', N'2017-02-17 15:47:37.000', N'全球google key')
GO
GO
INSERT INTO [dbo].[game_service_info] ([game_service_id], [service_type], [service_status], [service_app_id], [service_secret], [reg_date], [for_test]) VALUES (N'1', N'103', N'0', N'371278319899416', N'718013615b771c2d6cdcee3f76ed9ef8', N'2017-02-17 15:47:37.000', N'全球facebook key')
GO
GO
INSERT INTO [dbo].[game_service_info] ([game_service_id], [service_type], [service_status], [service_app_id], [service_secret], [reg_date], [for_test]) VALUES (N'1', N'11000', N'0', N'com.mfun.soul', N'', N'2016-08-08 15:47:37.500', N'Kr_aOS_Google')
GO
GO
INSERT INTO [dbo].[game_service_info] ([game_service_id], [service_type], [service_status], [service_app_id], [service_secret], [reg_date], [for_test]) VALUES (N'1', N'12000', N'0', N'com.mfun.soulsea', N'', N'2016-09-03 14:29:31.290', N'Kr_iOS_Appstore')
GO
GO
INSERT INTO [dbo].[game_service_info] ([game_service_id], [service_type], [service_status], [service_app_id], [service_secret], [reg_date], [for_test]) VALUES (N'1', N'13000', N'1', N'OA00705340', N'', N'2016-09-26 17:42:21.503', N'老的OneStore的')
GO
GO
INSERT INTO [dbo].[game_service_info] ([game_service_id], [service_type], [service_status], [service_app_id], [service_secret], [reg_date], [for_test]) VALUES (N'1', N'20100', N'0', N'', N'Darkblaze_Live_Push_Certi.pem', N'2016-10-02 19:27:36.590', null)
GO
GO
INSERT INTO [dbo].[game_service_info] ([game_service_id], [service_type], [service_status], [service_app_id], [service_secret], [reg_date], [for_test]) VALUES (N'1', N'20200', N'0', N'', N'AIzaSyDW4HBgOGR74YxwxaFHPLAjNZ1-48W21Pw', N'2016-08-12 00:00:00.000', null)
GO
GO
INSERT INTO [dbo].[game_service_info] ([game_service_id], [service_type], [service_status], [service_app_id], [service_secret], [reg_date], [for_test]) VALUES (N'1', N'21000', N'0', N'com.mfun.soul', N'', N'2017-02-23 15:47:37.000', N'全球aos包名')
GO
GO
INSERT INTO [dbo].[game_service_info] ([game_service_id], [service_type], [service_status], [service_app_id], [service_secret], [reg_date], [for_test]) VALUES (N'1', N'22000', N'0', N'com.mfun.soulsea', N'', N'2017-02-24 14:29:31.000', N'全球ios包名')
GO
GO
INSERT INTO [dbo].[game_service_info] ([game_service_id], [service_type], [service_status], [service_app_id], [service_secret], [reg_date], [for_test]) VALUES (N'1', N'1020000', N'0', N'335678673396-gcl2ab75h0jqa8evs411k574pc7iv58f.apps.googleusercontent.com', N'', N'2016-08-08 15:47:37.500', N'韩国的老Key')
GO
GO
INSERT INTO [dbo].[game_service_info] ([game_service_id], [service_type], [service_status], [service_app_id], [service_secret], [reg_date], [for_test]) VALUES (N'1', N'10300001', N'0', N'371278319899416', N'718013615b771c2d6cdcee3f76ed9ef8', N'2017-02-17 15:47:37.000', N'韩国的老Key')
GO
GO
INSERT INTO [dbo].[game_service_info] ([game_service_id], [service_type], [service_status], [service_app_id], [service_secret], [reg_date], [for_test]) VALUES (N'1', N'22000000', N'0', N'com.mfun.soulsea', N'', N'2017-02-24 14:29:31.000', N'zorro的老包名google')
GO
GO
INSERT INTO [dbo].[game_service_info] ([game_service_id], [service_type], [service_status], [service_app_id], [service_secret], [reg_date], [for_test]) VALUES (N'1', N'102000000', N'0', N'717076133499-4dalt6hpel8b68tepcoabb2mj1u55eji.apps.googleusercontent.com', N'wZWK2zwzFpGmGLEm5WYTZWwA', N'2017-02-17 15:47:37.000', N'zorro的老key google')
GO
GO
INSERT INTO [dbo].[game_service_info] ([game_service_id], [service_type], [service_status], [service_app_id], [service_secret], [reg_date], [for_test]) VALUES (N'1', N'1030000002', N'0', N'384863228546943', N'9b1605c90ffcc45a51b2c91148068c1e', N'2017-02-17 15:47:37.000', N'zorro全球facebook key')
GO
GO

-- ----------------------------
-- Table structure for game_service_product_id
-- ----------------------------
DROP TABLE [dbo].[game_service_product_id]
GO
CREATE TABLE [dbo].[game_service_product_id] (
[product_index] bigint NOT NULL IDENTITY(1,1) ,
[game_service_id] bigint NOT NULL ,
[billing_platform_type] int NOT NULL ,
[product_id] nvarchar(256) NOT NULL ,
[price_value] int NOT NULL ,
[price_tier] int NOT NULL 
)


GO

-- ----------------------------
-- Records of game_service_product_id
-- ----------------------------
SET IDENTITY_INSERT [dbo].[game_service_product_id] ON
GO
SET IDENTITY_INSERT [dbo].[game_service_product_id] OFF
GO

-- ----------------------------
-- Table structure for system_error_code
-- ----------------------------
DROP TABLE [dbo].[system_error_code]
GO
CREATE TABLE [dbo].[system_error_code] (
[error_code] nvarchar(64) NOT NULL ,
[error_number] bigint NOT NULL 
)


GO

-- ----------------------------
-- Records of system_error_code
-- ----------------------------

-- ----------------------------
-- Table structure for system_push_service
-- ----------------------------
DROP TABLE [dbo].[system_push_service]
GO
CREATE TABLE [dbo].[system_push_service] (
[push_id] bigint NOT NULL IDENTITY(1,1) ,
[game_service_id] bigint NOT NULL ,
[push_type] tinyint NOT NULL ,
[title] nvarchar(32) NOT NULL ,
[message] nvarchar(123) NOT NULL ,
[push_status] tinyint NOT NULL ,
[push_reason] nvarchar(MAX) NOT NULL ,
[send_reserv_date] datetime NOT NULL ,
[register] nvarchar(64) NOT NULL ,
[reg_date] datetime NOT NULL 
)


GO
DBCC CHECKIDENT(N'[dbo].[system_push_service]', RESEED, 377)
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'system_push_service', 
'COLUMN', N'push_id')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'푸쉬아이디'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'system_push_service'
, @level2type = 'COLUMN', @level2name = N'push_id'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'푸쉬아이디'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'system_push_service'
, @level2type = 'COLUMN', @level2name = N'push_id'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'system_push_service', 
'COLUMN', N'game_service_id')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'게임서비스 id'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'system_push_service'
, @level2type = 'COLUMN', @level2name = N'game_service_id'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'게임서비스 id'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'system_push_service'
, @level2type = 'COLUMN', @level2name = N'game_service_id'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'system_push_service', 
'COLUMN', N'push_type')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'푸쉬 타입 : 0 개발용 푸쉬, 1 배포용 푸쉬'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'system_push_service'
, @level2type = 'COLUMN', @level2name = N'push_type'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'푸쉬 타입 : 0 개발용 푸쉬, 1 배포용 푸쉬'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'system_push_service'
, @level2type = 'COLUMN', @level2name = N'push_type'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'system_push_service', 
'COLUMN', N'message')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'푸쉬 메시지'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'system_push_service'
, @level2type = 'COLUMN', @level2name = N'message'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'푸쉬 메시지'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'system_push_service'
, @level2type = 'COLUMN', @level2name = N'message'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'system_push_service', 
'COLUMN', N'push_status')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'발송상태 push_status : 0 중지 1 미확인 2 진행 3 완료'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'system_push_service'
, @level2type = 'COLUMN', @level2name = N'push_status'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'발송상태 push_status : 0 중지 1 미확인 2 진행 3 완료'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'system_push_service'
, @level2type = 'COLUMN', @level2name = N'push_status'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'system_push_service', 
'COLUMN', N'push_reason')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'푸쉬 타입(발송 사유?) push_type : 1 정기 Push, 2 Event Push'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'system_push_service'
, @level2type = 'COLUMN', @level2name = N'push_reason'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'푸쉬 타입(발송 사유?) push_type : 1 정기 Push, 2 Event Push'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'system_push_service'
, @level2type = 'COLUMN', @level2name = N'push_reason'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'system_push_service', 
'COLUMN', N'send_reserv_date')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'발송 예약일'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'system_push_service'
, @level2type = 'COLUMN', @level2name = N'send_reserv_date'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'발송 예약일'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'system_push_service'
, @level2type = 'COLUMN', @level2name = N'send_reserv_date'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'system_push_service', 
'COLUMN', N'register')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'등록자'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'system_push_service'
, @level2type = 'COLUMN', @level2name = N'register'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'등록자'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'system_push_service'
, @level2type = 'COLUMN', @level2name = N'register'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'system_push_service', 
'COLUMN', N'reg_date')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'등록일'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'system_push_service'
, @level2type = 'COLUMN', @level2name = N'reg_date'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'등록일'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'system_push_service'
, @level2type = 'COLUMN', @level2name = N'reg_date'
GO

-- ----------------------------
-- Records of system_push_service
-- ----------------------------
SET IDENTITY_INSERT [dbo].[system_push_service] ON
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'1', N'1', N'1', N'열쇠 60개, 선물 도착!', N'열쇠가 도착했습니다. 게임 내 [선물 목록]에서 수령 받고, 게임을 즐기세요. ~', N'3', N'18시 열쇠 선물 안내 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-08-18 16:34:23.173')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'2', N'1', N'1', N'열쇠 60개, 선물 도착!', N'열쇠가 도착했습니다. 게임 내 [선물 목록]에서 수령 받으시고, 던전으로 고고고!', N'3', N'21시 열쇠 선물 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-08-18 16:37:39.267')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'3', N'1', N'1', N'금요일 아침! 열쇠 선물 도착', N'신나는 금요일 아침, 게임 내 [선물 목록]에서 잊지말고 열쇠 챙기세요. ^~^', N'3', N'08시 열쇠 선물 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-08-18 16:59:10.693')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'4', N'1', N'1', N'점심 시간! 다크블레이즈 한판', N'열쇠가 도착했습니다. 게임 내 [선물 목록]에서 수령 받으시고, 던전으로 고고고!', N'3', N'12시, 열쇠 선물 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-08-18 17:09:09.263')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'5', N'1', N'1', N'오늘이~ 불금인가요?!', N'불금 저녁 타임의 시작! 잊지 말고 [선물 목록]에서 열쇠 챙기세요~', N'3', N'18시, 열쇠 선물 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-08-18 17:11:23.697')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'6', N'1', N'1', N'열쇠가 필요하지 않으신가요?', N'금요일! 오늘의 마지막 열쇠 선물! [선물 목록]에서 잊지 말고 수령 받으세요~', N'3', N'21시, 열쇠 선물 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-08-18 17:15:52.153')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'7', N'1', N'1', N'열쇠 부족하시죠?', N'조금 더 즐기실수 있도록 추가 열쇠 60개 우편으로 보내드려요~', N'3', N'18일, 19시 추가 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-08-18 17:57:10.833')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'8', N'1', N'1', N'열쇠 부족하시죠?', N'조금 더 즐기실수 있도록 추가 열쇠 60개 우편으로 보내드려요~', N'3', N'19일, 19시 푸시 세팅', N'2017-03-04 14:00:00.000', N'test2', N'2016-08-18 21:44:43.970')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'9', N'1', N'1', N'점검 보상 도착!', N'점검 진행으로 불편을 드려죄송합니다. 우편함에서 보상 수령 받으세요.', N'3', N'8/19일, 임시 점검 보상', N'2017-03-04 14:00:00.000', N'test2', N'2016-08-19 18:28:29.730')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'10', N'1', N'1', N'열쇠 60개, 선물 도착!', N'열쇠가 도착했습니다. 게임 내 [선물 목록]에서 수령 받고, 게임을 즐기세요. ~', N'3', N'20일, 8시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-08-19 22:40:06.153')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'11', N'1', N'1', N'점심 먹고, 열쇠 받고!', N'토요일 점심 시간에도 열쇠는 쭈~욱 배달 옵니다. [선물 목록]에서 수령 받으세요.', N'3', N'20일, 12시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-08-19 22:41:40.190')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'12', N'1', N'1', N'토요일 저녁 타임! 열쇠 도착', N'열쇠가 도착했습니다. 게임 내 [선물 목록]에서 수령 받고, 게임을 즐기세요. ~', N'3', N'20일, 18시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-08-19 22:42:42.953')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'13', N'1', N'1', N'강화 재료 부족하시죠?', N'강화 하는데 도움 되라고 철 조각 우편함에 넣어 드렸어요.~ ♥', N'3', N'20일, 19시 별도 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-08-19 22:44:20.547')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'14', N'1', N'1', N'토요일은 열쇠가 좋아~!', N'즐거운 토요일 밤~ [선물 목록]에서 잊지 말고 열쇠 받으세요~', N'3', N'20일, 21시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-08-19 22:45:34.797')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'15', N'1', N'1', N'썬데이 모닝! 열쇠 도착!', N'일요일이라고 늦잠만 주무시지 말고, [선물 목록]에서 열쇠 받으세요~', N'3', N'21일, 8시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-08-19 22:46:48.917')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'16', N'1', N'1', N'열쇠 받고 갑시다!', N'열쇠가 도착했습니다. 게임 내 [선물 목록]에서 수령 받고, 게임을 즐기세요. ~', N'3', N'21일, 12시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-08-19 22:48:04.780')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'17', N'1', N'1', N'열~쇠 받아요~!', N'[선물 목록]에 열쇠가 도착했습니다.~', N'3', N'21일, 18시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-08-19 22:49:21.473')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'18', N'1', N'1', N'강화 재료 선물 도착!', N'무기 강화 성공 하시라고 강화석 우편함에 숨겨뒀어요~♡', N'3', N'21일, 19시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-08-19 22:50:47.407')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'19', N'1', N'1', N'휴일 끝자락에도 열쇠는 온다', N'잊지 말고 [선물 목록]에서 열쇠 수령 받으시고, 던전으로 고고고~', N'3', N'21일, 21시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-08-19 22:52:35.487')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'20', N'1', N'1', N'월요일 아침 힘내세요!', N'열쇠가 도착했습니다. 게임 내 [선물 목록]에서 수령 받고, 게임을 즐기세요. ~', N'3', N'22일, 8시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-08-19 22:53:36.657')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'21', N'1', N'1', N'열쇠 받고~ 점심 먹고~', N'점심 휴식 시간 ~ [선물 목록]에서 열쇠 받고, 다크블레이즈 한판!', N'3', N'22일, 12시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-08-19 22:54:40.387')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'22', N'1', N'1', N'열쇠가 부족하다!', N'그렇다면 지금 바로 [선물 목록]에서 열쇠 받으세요~', N'3', N'22일, 18시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-08-19 22:55:40.433')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'23', N'1', N'1', N'열쇠가~ 도착했다!', N'잊지말고 [선물 목록]에서 열쇠를 찾아가세요.~', N'3', N'22일, 21시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-08-19 22:56:55.277')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'24', N'1', N'1', N'구글 인기 순위 2위 달성!', N'공식 카페에서 구글 인기 순위 2위 달성 보상 공지 확인하고 쿠폰 받으세요.~', N'3', N'구글 인기 순위 달성 관련 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-08-22 20:27:20.400')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'25', N'1', N'1', N'상쾌한 아침! 열쇠 60개!', N'아침 선물로 열쇠 60개 도착! [선물 목록]에서 수령 받으세요~', N'3', N'23일, 8시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-08-22 21:59:14.383')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'26', N'1', N'1', N'맛점 하세요~ 열쇠는 선물~', N'[선물 목록]에 열쇠가 도착했습니다.~', N'3', N'23일, 12시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-08-22 22:00:10.437')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'27', N'1', N'1', N'열쇠 60개, 선물 도착!', N'열쇠가 도착했습니다. 게임 내 [선물 목록]에서 수령 받고, 게임을 즐기세요. ~', N'3', N'23일, 18시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-08-22 22:06:20.263')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'28', N'1', N'1', N'늦은 밤, 열쇠 도착!', N'[선물 목록]에 도착한 열쇠 챙기시고~ 게임도 즐기시고~', N'3', N'23일, 21시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-08-22 22:23:40.820')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'29', N'1', N'1', N'열~쇠 받아요~!', N'[선물 목록]에 열쇠가 도착했습니다.~', N'3', N'24일, 8시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-08-24 00:13:41.973')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'30', N'1', N'1', N'열쇠 받고~ 점심 먹고~', N'점심 휴식 시간 ~ [선물 목록]에서 열쇠 받고, 다크블레이즈 한판!', N'3', N'24일, 12시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-08-24 00:14:10.980')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'31', N'1', N'1', N'열쇠 받고 갑시다!', N'잊지 말고 [선물 목록]에서 열쇠 수령 받으시고, 던전으로 고고고~', N'3', N'24일, 18시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-08-24 00:14:42.463')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'32', N'1', N'1', N'늦은 밤, 열쇠 도착!', N'잠자리 들기전 [선물 목록]에서 열쇠 받고, 던전 한판 즐기시죠~', N'3', N'24일, 21시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-08-24 00:15:38.150')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'33', N'1', N'1', N'출근길~ 열쇠 도착!', N'던전 재미있게 달리시라고 [선물 목록]에 열쇠 60개 도착!', N'3', N'25일, 8시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-08-24 21:55:01.347')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'34', N'1', N'1', N'점심 시간! 다크블레이즈 한판', N'점심 휴식 시간 ~ [선물 목록]에서 열쇠 받고, 다크블레이즈 한판!', N'3', N'25일, 12시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-08-24 21:56:13.540')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'35', N'1', N'1', N'퇴근길! 다크블레이즈와 함께~', N'열쇠가 도착했습니다. 게임 내 [선물 목록]에서 수령 받고, 던전으로 고고고!', N'3', N'25일, 18시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-08-24 22:01:55.340')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'36', N'1', N'1', N'열쇠 60개, 선물 도착!', N'오늘 하루 마무리 전, [선물 목록]에서 열쇠 받고 다크블레이즈 한판~', N'3', N'25일, 21시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-08-24 22:03:01.283')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'37', N'1', N'1', N'금요일 아침! 열쇠 선물 도착', N'잊지 말고 [선물 목록]에서 열쇠 수령 받으시고, 던전으로 고고고~', N'3', N'26일, 8시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-08-26 01:09:12.570')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'38', N'1', N'1', N'열쇠 60개, 선물 도착!', N'[선물 목록]에 도착한 열쇠 챙기시고~ 게임도 즐기시고~', N'3', N'26일, 12시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-08-26 01:09:55.513')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'39', N'1', N'1', N'열쇠 받고 갑시다!', N'[선물 목록]에 열쇠가 도착했습니다.~', N'3', N'26일, 18시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-08-26 01:11:17.703')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'40', N'1', N'1', N'금요일 밤, 열쇠 도착!', N'불타는 금요일 밤, 마지막 열쇠 도착! [선물 목록]에서 수령 받으세요.', N'3', N'26일, 21시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-08-26 01:12:12.217')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'41', N'1', N'1', N'토요일 아침! 열쇠 도착!', N'[선물 목록]에 열쇠가 도착했습니다.~', N'3', N'27일, 8시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-08-26 20:33:02.167')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'42', N'1', N'1', N'맛점 하세요~ 열쇠는 선물~', N'[선물 목록]에서 열쇠 받고, 신나게 다크블레이즈 한판!', N'3', N'27일, 12시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-08-26 20:33:44.853')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'43', N'1', N'1', N'토요일 저녁 타임! 열쇠 도착', N'잊지 말고 [선물 목록]에서 열쇠 수령 받으시고, 던전으로 고고고~', N'3', N'27일, 18시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-08-26 20:34:30.400')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'44', N'1', N'1', N'주말! 루비 선물!', N'우편함에 50루비 선물로 보내 드려요.~', N'3', N'27일, 19시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-08-26 20:35:23.237')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'45', N'1', N'1', N'토요일 밤이 좋아~ 열쇠 좋아', N'열쇠가 도착했습니다. 게임 내 [선물 목록]에서 수령 받고, 게임을 즐기세요. ~', N'3', N'27일, 21시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-08-26 20:36:26.393')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'46', N'1', N'1', N'일요일 아침~ 열쇠 선물', N'[선물 목록]에 도착한 열쇠 챙기시고~ 게임도 즐기시고~', N'3', N'28일, 8시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-08-26 20:37:18.290')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'47', N'1', N'1', N'점심 시간! 다크블레이즈 한판', N'잊지 말고 [선물 목록]에서 열쇠 수령 받으시고, 던전으로 고고고~', N'3', N'28일, 12시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-08-26 20:38:01.853')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'48', N'1', N'1', N'열쇠 60개, 선물 도착!', N'[선물 목록]에서 열쇠 꼭 챙기세요 ~', N'3', N'28일, 18시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-08-26 20:39:11.517')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'49', N'1', N'1', N'일요일 저녁, 루비 선물', N'선물로 50루비 보내드려요.~ 우편함을 확인하세요~', N'3', N'28일, 19시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-08-26 20:40:17.653')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'50', N'1', N'1', N'늦은 밤, 열쇠 도착!', N'아쉬운 주말 저녁, [선물 목록]에서 열쇠 받고 다크블레이즈 한판 즐기시죠~', N'3', N'28일, 21시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-08-26 20:41:29.830')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'51', N'1', N'1', N'월요일 아침 힘내세요!', N'열쇠가 도착했습니다. 게임 내 [선물 목록]에서 수령 받고, 게임을 즐기세요. ~', N'3', N'29일, 8시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-08-29 00:35:26.840')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'52', N'1', N'1', N'점심 시간! 다크블레이즈 한판', N'점심 휴식 시간 ~ [선물 목록]에서 열쇠 받고, 다크블레이즈 한판!', N'3', N'29일, 12시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-08-29 00:35:53.743')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'53', N'1', N'1', N'열쇠가~ 도착했다!', N'[선물 목록]에 열쇠가 도착했습니다.~', N'3', N'29일, 18시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-08-29 00:36:25.470')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'54', N'1', N'1', N'늦은 밤, 열쇠 도착!', N'열쇠가 도착했습니다. 게임 내 [선물 목록]에서 수령 받고, 던전으로 고고고!', N'3', N'29일, 21시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-08-29 00:36:52.430')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'55', N'1', N'1', N'오늘도 힘차게!', N'출근길 선물 도착~ [선물 목록]에 도착한 열쇠 받으세요~', N'3', N'30일, 8시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-08-29 23:18:40.217')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'56', N'1', N'1', N'맛점 하세요~ 열쇠는 선물~', N'점심 휴식 시간 ~ [선물 목록]에서 열쇠 받고, 다크블레이즈 한판!', N'3', N'30일, 12시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-08-29 23:19:21.290')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'57', N'1', N'1', N'퇴근길 열쇠 선물 도착!', N'열쇠가 도착했습니다. 게임 내 [선물 목록]에서 수령 받고, 던전으로 고고고!', N'3', N'30일, 18시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-08-29 23:20:03.330')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'58', N'1', N'1', N'오늘의 마지막 열쇠!', N'잊지 말고 [선물 목록]에서 열쇠 수령 받으시고, 던전으로 고고고~', N'3', N'30일, 21시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-08-29 23:20:38.500')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'59', N'1', N'1', N'출근길 선물 도착~', N'[선물 목록]에 열쇠가 도착했습니다.~', N'3', N'31일, 8시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-08-30 22:58:04.870')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'60', N'1', N'1', N'열쇠가~ 도착했다!', N'[선물 목록]에 도착한 열쇠 챙기시고~ 게임도 즐기시고~', N'3', N'31일, 18시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-08-30 22:58:45.150')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'61', N'1', N'1', N'열쇠 60개, 선물 도착!', N'열쇠가 도착했습니다. 게임 내 [선물 목록]에서 수령 받고, 게임을 즐기세요. ~', N'3', N'31일, 21시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-08-30 23:15:36.420')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'62', N'1', N'1', N'상쾌한 아침! 열쇠 60개!', N'[선물 목록]에 도착한 열쇠 챙기시고~ 게임도 즐기시고~', N'3', N'9/1, 8시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-09-01 02:44:23.860')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'63', N'1', N'1', N'맛점 하세요~ 열쇠는 선물~', N'점심 휴식 시간 ~ [선물 목록]에서 열쇠 받고, 다크블레이즈 한판!', N'3', N'9/1, 12시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-09-01 02:44:58.997')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'64', N'1', N'1', N'열쇠 도착!', N'열쇠가 도착했습니다. 게임 내 [선물 목록]에서 수령 받으세요~', N'3', N'9/1, 18시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-09-01 02:45:46.443')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'65', N'1', N'1', N'늦은 밤, 열쇠 도착!', N'오늘 하루 마무리 전, [선물 목록]에서 열쇠 받고 다크블레이즈 한판~', N'3', N'9/1, 21시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-09-01 02:46:38.727')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'66', N'1', N'1', N'출근길 선물 도착~', N' 출근길 선물 도착~ [선물 목록]에 도착한 열쇠 받으세요~', N'3', N'9/2, 8시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-09-01 22:48:17.113')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'67', N'1', N'1', N'맛점 하세요~ 열쇠는 선물~', N'잊지 말고 [선물 목록]에서 열쇠 수령 받으시고, 던전으로 고고고~', N'3', N'9/2, 12시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-09-01 22:58:01.407')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'68', N'1', N'1', N'퇴근길 열쇠 선물 도착!', N'퇴근길 선물 도착~ [선물 목록]에 도착한 열쇠 받으세요~', N'3', N'9/2, 18시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-09-01 22:58:27.890')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'69', N'1', N'1', N'늦은 밤, 열쇠 도착!', N'조금 더 즐기실수 있도록 추가 열쇠 60개 우편으로 보내드려요~', N'3', N'9/2, 21시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-09-01 22:59:07.477')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'70', N'1', N'1', N'토요일 아침! 열쇠 선물 도착', N'신나는 토요일 아침, 게임 내 [선물 목록]에서 잊지말고 열쇠 챙기세요. ^~^', N'3', N'9/3, 8시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-09-02 15:46:21.920')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'71', N'1', N'1', N'점심 시간! 다크블레이즈 한판', N'[선물 목록]에서 열쇠 받고, 다크블레이즈 한판!', N'3', N'9/3, 12시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-09-02 15:49:13.290')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'72', N'1', N'1', N'토요일 저녁 타임! 열쇠 도착', N'잊지 말고 [선물 목록]에서 열쇠 수령 받으시고, 던전으로 고고고~', N'3', N'9/3, 18시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-09-02 15:51:08.630')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'73', N'1', N'1', N'열쇠 60개, 선물 도착!', N'열쇠가 도착했습니다. 게임 내 [선물 목록]에서 수령 받고, 게임을 즐기세요. ~', N'3', N'9/3, 21시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-09-02 15:52:32.520')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'74', N'1', N'1', N'늦잠 No! 열쇠 Yes!', N'잊지 말고 [선물 목록]에서 열쇠 수령 받으시고, 던전으로 고고고~', N'3', N'9/4, 8시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-09-02 15:53:51.190')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'75', N'1', N'1', N'일요일 오후 즐겁게!', N'[선물 목록]에 도착한 열쇠 챙기시고~ 게임도 즐기시고~', N'3', N'9/4, 12시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-09-02 15:54:30.163')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'76', N'1', N'1', N'열~쇠 받아요~!', N'[선물 목록]에 열쇠가 도착했습니다.~', N'3', N'9/4, 18시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-09-02 15:55:04.813')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'77', N'1', N'1', N'휴일 끝자락 열쇠 선물!', N'[선물 목록]에 도착한 열쇠 미리 미리 챙기세요~', N'3', N'9/4, 21시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-09-02 15:56:47.000')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'78', N'1', N'1', N'주말 선물 도착~', N'주말 선물로 우편함에 루비 80개 넣어드려요~', N'3', N'주말 별도 보상', N'2017-03-04 14:00:00.000', N'test2', N'2016-09-02 19:23:38.793')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'79', N'1', N'1', N'일요일 선물 도착!', N'일요일 저녁 선물로 루비 80개 우편함에 넣어드려요~', N'3', N'주말 별도 보상', N'2017-03-04 14:00:00.000', N'test2', N'2016-09-02 19:24:20.310')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'80', N'1', N'1', N'퇴근길 열쇠 선물 도착!', N'잊지 말고 [선물 목록]에서 열쇠 수령 받으시고, 던전으로 고고고~', N'3', N'9/5, 18시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-09-05 16:24:53.103')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'81', N'1', N'1', N'늦은 밤, 열쇠 도착!', N'열쇠가 도착했습니다. 게임 내 [선물 목록]에서 수령 받고, 던전으로 고고고!', N'3', N'9/5, 21시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-09-05 16:25:49.500')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'82', N'1', N'1', N'출근길 선물 도착~', N' 출근길 선물 도착~ [선물 목록]에 도착한 열쇠 받으세요~', N'3', N'9/6, 8시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-09-05 16:28:13.313')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'83', N'1', N'1', N'점심 시간! 다크블레이즈 한판', N'점심 휴식 시간 ~ [선물 목록]에서 열쇠 받고, 다크블레이즈 한판!', N'3', N'9/6, 12시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-09-05 16:29:00.580')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'84', N'1', N'1', N'퇴근길 열쇠 선물 도착!', N'퇴근길 무료하지 않게, [선물 목록]에 열쇠 60개 도착~', N'3', N'9/6, 18시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-09-05 16:30:15.207')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'85', N'1', N'1', N'열~쇠 받아요~!', N'오늘 하루 마무리 전, [선물 목록]에서 열쇠 받고 다크블레이즈 한판~', N'3', N'9/6, 21시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-09-05 16:31:09.203')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'86', N'1', N'1', N'수요일 열쇠 도착했수요~! ', N'수요일 열쇠 도착했수요!! [선물 목록]에서 열쇠 받아가수요! ', N'3', N'9/7, 8시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-09-06 19:02:58.477')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'87', N'1', N'1', N'점검 끝났으니 시원하게 한판!', N'점검 마치고 한판 해볼까? [선물 목록]에 가면 열쇠 선물이 짠!', N'3', N'9/6, 12시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-09-06 19:04:48.880')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'88', N'1', N'1', N'칼퇴하고 다크블레이즈 한판!', N'영웅님들의 퇴근길을 책임질 다크블레이즈! [선물 목록]에서 열쇠 받아가세요!', N'3', N'9/7, 18시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-09-06 19:06:40.577')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'89', N'1', N'1', N'수요일밤을 책임질 다크블레이즈', N'[열쇠 목록]에서 열쇠 받고 자기전 다블 고고씽!', N'0', N'9/6, 21시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-09-06 19:10:41.220')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'90', N'1', N'1', N'수요일밤을 책임질 다크블레이즈', N'[선물 목록]에서 열쇠 받고 자기전 다블 고고씽!', N'3', N'9/7, 21시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-09-06 19:19:41.933')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'91', N'1', N'1', N'출근길도 다크블레이즈와 함께!', N'출근길에 [선물목록]에서 열쇠 받아가세요~', N'3', N'9/8, 8시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-09-07 15:42:41.493')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'92', N'1', N'1', N'점심시간 열쇠 받으세요~', N'점심드시고 [선물목록]에서 열쇠 받으셔서 다크블레이즈 한판 하세요~', N'3', N'9/8, 12시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-09-07 15:47:04.647')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'93', N'1', N'1', N'신나는 목요일 저녁에도 다블!', N'목요일 저녁, 다크블레이즈 접속하셔서 [선물 목록]의 열쇠 받아가세요~!', N'3', N'9/8, 18시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-09-07 15:50:49.780')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'94', N'1', N'1', N'다크블레이즈와 함께 불목!', N'[선물 목록]에서 열쇠 받으시고 다크블레이즈와 불목하세요~!', N'3', N'9/8, 21시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-09-07 15:51:32.797')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'95', N'1', N'1', N'굿모닝~! 열쇠 받으세요~', N'[선물 목록]에서 열쇠 받고 즐거운 하루 보내세요~!', N'3', N'9/9, 8시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-09-08 12:01:45.317')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'96', N'1', N'1', N'우와~! 벌써 점심시간~!', N'맛점하시구 다크블레이즈 한판! [선물 목록', N'0', N'test', N'2017-03-04 14:00:00.000', N'test2', N'2016-09-08 12:35:13.810')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'97', N'1', N'1', N'우와~! 벌써 점심시간~!', N'맛점하시구 다크블레이즈 한판! [선물 목록]에서 열쇠 받아가세요~!', N'3', N'9/9, 12시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-09-08 12:35:59.600')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'98', N'1', N'1', N'금요일 저녁, 열쇠 도착!', N'불금 저녁의 시작! [선물 목록]에서 열쇠 받고 불금 시작하세요~', N'3', N'9/9, 18시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-09-08 12:39:15.757')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'99', N'1', N'1', N'오늘밤도 열쇠 선물~!', N'불타는 금요일! [선물 목록]에서 열쇠 받고 금요일 밤을 불태워봅시다~', N'3', N'9/9, 21시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-09-08 12:40:41.920')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'100', N'1', N'1', N'토요일 아침 꿀모닝 보상', N'꿀잠자고 다크블레이즈 한판! 열쇠 보상 받아가세요~', N'3', N'test', N'2017-03-04 14:00:00.000', N'test2', N'2016-09-09 17:05:54.663')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'101', N'1', N'1', N'다크블레이즈와 함께하는 오후', N'아점 먹고 나른한 오후 다크블레이즈 한판! [선물 목록]에 열쇠가 도착했어요!', N'3', N'test', N'2017-03-04 14:00:00.000', N'test2', N'2016-09-09 17:07:23.360')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'102', N'1', N'1', N'불타는 토요일은? 다블과 함께', N'지금 접속하시면 열쇠 선물이 팡! [선물 목록]고고씽!', N'3', N'test', N'2017-03-04 14:00:00.000', N'test2', N'2016-09-09 17:08:26.863')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'103', N'1', N'1', N'토요일밤에~ 다블 한판해~♬', N'[선물 목록]에 가면 열쇠 선물이 와르르~~~!!', N'3', N'test', N'2017-03-04 14:00:00.000', N'test2', N'2016-09-09 17:17:17.400')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'104', N'1', N'1', N'주말 아침8시에 푸시보내기!!', N'영웅님! 일어나셔서 열쇠선물 받으시고 다블하러 가요~♥', N'3', N'test', N'2017-03-04 14:00:00.000', N'test2', N'2016-09-09 17:18:18.860')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'105', N'1', N'1', N'일요일엔 짜*게티먹고 다블!', N'오늘은 내가 다블의 요리사! 열쇠받고 다블해요♥', N'3', N'test', N'2017-03-04 14:00:00.000', N'test2', N'2016-09-09 17:24:35.217')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'106', N'1', N'1', N'일요일저녁 다블과 함께 꿀휴식', N'맛있는 저녁 드시고 다블이 드리는 열쇠선물 확인하기!♥', N'3', N'test', N'2017-03-04 14:00:00.000', N'test2', N'2016-09-09 17:26:43.580')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'107', N'1', N'1', N'일요일밤도 다크블레이즈!', N'자기 전 열쇠받고 다블이 생각 한번☞☜', N'3', N'test', N'2017-03-04 14:00:00.000', N'test2', N'2016-09-09 17:27:57.940')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'108', N'1', N'1', N'월요병 다블이와 함께 극뽀옥~', N'다블이가 드리는 열쇠선물 받으시고 오늘도 화이팅!♥', N'3', N'test', N'2017-03-04 14:00:00.000', N'test2', N'2016-09-09 17:31:17.450')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'109', N'1', N'1', N'맛점 하시고 다블 한판!', N'♥열쇠 선물 도착♥', N'3', N'9/12 오후12시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-09-09 17:32:45.897')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'110', N'1', N'1', N'오늘도 너무 수고하신 영웅님께', N'다블이가 드리는 열쇠 선물이 도착! 수고하셨습니다♥', N'3', N'9/12 18시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-09-09 17:33:24.940')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'111', N'1', N'0', N'월요일의 피로를 씻어줄 한판!', N'[선물 목록]에 열쇠 받으시고 자기전 한판 고고띵!', N'0', N'9/12 오후 21시 푸시', N'2017-03-04 13:45:00.000', N'test2', N'2016-09-09 17:34:19.270')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'112', N'1', N'1', N'월요일의 피로를 씻어줄 한판!', N'[선물 목록]에 열쇠 받으시고 자기전 한판 고고띵!', N'3', N'9/12 오후 21시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-09-09 17:36:19.890')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'113', N'1', N'1', N'상쾌한 아침에 다블 한판!', N'다크블레이즈 접속하셔서 [선물 목록]을 보면 열쇠 선물이 짠!!', N'3', N'9/13, 8시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-09-12 19:04:44.697')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'114', N'1', N'1', N'맛점하고 선물 받기~', N'맛있는 점심 드셨나요~? 다블이도 배부르게 [선물목록]에서 열쇠 받아주세요~^^', N'3', N'9/13, 12시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-09-12 19:06:16.473')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'115', N'1', N'1', N'다블이와 저녁 만찬!', N'[선물목록] 확인하시고 푸짐한 저녁 드세요~!', N'3', N'9/13, 18시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-09-12 19:09:29.103')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'116', N'1', N'1', N'연휴 전날도 다블 Go Go!', N'긴~ 연휴의 시작! 오늘도 잊지 말고 열쇠 받아가세요~^^', N'3', N'9/13, 21시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-09-12 19:11:01.247')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'117', N'1', N'1', N'추석 연휴도 열쇠 선물!', N'[선물목록]의 열쇠 받고~ 추석 연휴의 시작도 다블이와 함께하세요~', N'3', N'9/14, 8시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-09-13 16:46:49.927')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'118', N'1', N'1', N'점심시간~ 일어나서 다블!', N'연휴라고 늦잠 주무시지 마시고 어서 [선물목록]의 열쇠 받아가세요~', N'3', N'9/14, 12시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-09-13 16:51:07.377')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'119', N'1', N'1', N'열쇠가 왔습니다~', N'귀성길 지루하시죠? 다블 접속하셔서 [선물목록]의 열쇠 받고 게임 한판 하시는 건 어떠세요~?', N'3', N'9/14, 18시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-09-13 16:53:23.900')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'120', N'1', N'1', N'열쇠 받을 시간이에요~', N'열쇠 받는 것 잊지 않으셨죠~? 다크블레이즈 [선물목록]에서 열쇠 받아가세요~', N'3', N'9/14, 21시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-09-13 16:54:35.443')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'121', N'1', N'1', N'늦잠 No! 다블 Yes!', N'[선물목록]에 열쇠가 도착했습니다~ 선물 받고 또 주무세요~', N'3', N'9/15, 8시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-09-13 16:55:44.700')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'122', N'1', N'1', N'[선물목록] 확인하세요~', N'맛점 하셨나요~? 열쇠 받고 다블하면서 소화시키세요~', N'3', N'9/15, 12시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-09-13 16:57:00.293')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'123', N'1', N'1', N'띠리링! 선물이 도착했습니다.', N'열쇠 부족하셨죠? [선물목록]에 열쇠가 도착했습니다~', N'3', N'9/15, 18시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-09-13 16:58:04.277')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'124', N'1', N'1', N'연휴 + 불목엔~? 다블~!', N'즐겁고 신나는 밤에~ [선물목록]의 열쇠 받고 불목을 즐겨보세요~', N'3', N'9/15, 21시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-09-13 16:59:54.143')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'125', N'1', N'1', N'좋은 아침! 선물 왔어요~', N'졸린 눈 비비며 [선물목록]의 열쇠 받아서 다블하며 잠을 깨 보아요~♥', N'3', N'9/16, 8시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-09-13 17:02:28.637')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'126', N'1', N'1', N'선물목록에 뭔가 도착했어요~', N'짜잔~! [선물목록]에 열쇠가 도착했습니다~ 신나게 다블 한판 고고씽~!', N'3', N'9/16, 12시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-09-13 17:04:07.000')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'127', N'1', N'1', N'따르릉~따르릉~ 전화왔어요~♬', N'[선물목록]에 열쇠왔다고 전화왔어요~♪', N'3', N'9/16, 18시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-09-13 17:09:47.690')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'128', N'1', N'1', N'불타는 금요일 밤도 다블?!', N'다블에서 열쇠가 왔습니다~ [선물목록] 확인하시고 게임 한판 하세요~', N'3', N'9/16, 21시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-09-13 17:12:36.517')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'129', N'1', N'1', N'열쇠 받고 상쾌한 하루되세요~', N'[선물목록]에서 열쇠 받으시고 주말을 상쾌하게 시작하세요~', N'3', N'9/17, 8시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-09-13 17:15:11.403')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'130', N'1', N'1', N'토토가? 토토다!', N'토요일 토요일은 다크블레이즈! 줄여서 토토다~^^ 토요일도 [선물목록]에서 열쇠 받으세요', N'3', N'9/17, 12시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-09-13 17:16:48.367')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'131', N'1', N'1', N'택배 왔어요~', N'다크블레이즈에서 열쇠를 보냈답니다~ [선물목록]에서 확인해주세요~^^', N'3', N'9/17, 18시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-09-13 17:18:35.830')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'132', N'1', N'1', N'열쇠 60개가 짠~!', N'다크블레이즈 들어오셔서 [선물목록]을 확인하면? 열쇠 60개가 짠~! 들어왔답니다~^^', N'3', N'9/17, 21시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-09-13 17:21:02.450')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'133', N'1', N'1', N'일어나요~ 다블돌이~♪', N'아쉬운 연휴의 마지막 날, 일요일 아침입니다. 어서 일어나서 열쇠 선물 받으세요~^^', N'3', N'9/18, 8시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-09-13 17:23:09.710')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'134', N'1', N'1', N'맛점하시고 다블 한판!', N'[선물목록]의 열쇠 받으시고 다블과 함께 맛점하세요~!', N'3', N'9/18, 12시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-09-13 17:24:37.273')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'135', N'1', N'1', N'다크블레이즈 열쇠 도착!', N'[선물 목록]에 열쇠가 도착했습니다. 열쇠 받고 다블 한판 어떠세요~?', N'3', N'9/18, 18시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-09-13 17:26:26.987')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'136', N'1', N'1', N'열쇠 받고 연휴의 마무으리!', N'연휴의 마지막 날, 쓸쓸한 마음을 달래주려고 다블이가 열쇠 선물을 보냈습니다~ [선물목록] 확인해주세요^^', N'3', N'9/18, 21시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-09-13 17:27:59.250')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'137', N'1', N'1', N'오랜만의 출근길엔 다블 한판!', N'쉬었다가 출근하려니 힘드시죠...? 그럴 땐 다블이의 열쇠 선물 받으시고 게임하면서 기분전환하세요~', N'3', N'9/19, 8시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-09-13 17:33:10.207')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'138', N'1', N'1', N'지치고 힘들땐~ 다블 해요~♬', N'언제나 다블이~ 열쇠 드릴게요~♪ [선물 목록]의 열쇠 받고 오늘 하루도 힘내세요~^^', N'3', N'9/19, 12시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-09-13 17:34:56.720')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'139', N'1', N'1', N'열쇠 받아가세요~', N'기분 좋은 퇴근길, [선물목록]의 열쇠 받으시고 다블이랑 놀아주세요~', N'3', N'9/19, 18시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-09-13 17:35:53.873')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'140', N'1', N'1', N'선물이 도착했습니다!', N'오늘 하루도 열심히 보낸 영웅님께, 다블이가 [선물목록]에 열쇠 넣어두었답니다~', N'3', N'9/19, 21시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-09-13 17:37:08.200')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'141', N'1', N'1', N'화요일 아침 열쇠 선물♬', N'상큼한 화요일 다블이가 드리는 열쇠선물~♥', N'3', N'9/20, 8시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-09-19 19:16:07.827')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'142', N'1', N'1', N'꿀같은 점심시간 꿀같은 열쇠', N'맛점 하시구 다블이가 드리는 열쇠 [선물 목록]에서 확인해보세요 ^^', N'3', N'9/20, 12시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-09-19 20:06:37.430')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'143', N'1', N'1', N'영웅님들을 위한 선물 열쇠!', N'퇴근전 다크블레이즈 접속해서 열쇠 선물 받아가세요~~!!', N'3', N'9/20, 18시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-09-19 20:12:16.630')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'144', N'1', N'1', N'다블이와 함께 꿀잠♥', N'지금 열쇠 받으시고 자기 전 다블 한판! [선물 목록]을 확인해 주세요^^', N'3', N'9/20, 21시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-09-19 20:21:12.460')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'145', N'1', N'1', N'[선물목록] 열쇠 도착!', N'지금 다크블레이즈 [선물목록]에 열쇠가 도착했답니다~ 늦지 않게 꼭 받아가세요~!', N'3', N'9/21, 8시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-09-20 19:14:11.453')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'146', N'1', N'1', N'맛점하고 다블 한판~!', N'오늘 점심은 "열쇠" 반찬과 함께~ 맛점하시구 다블하세요~!', N'3', N'9/21, 12시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-09-20 19:15:27.403')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'147', N'1', N'1', N'퇴근길을 더욱 신나게!', N'[선물목록]의 열쇠 받으시고 신나게 다크블레이즈 하세요~', N'3', N'9/21, 18시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-09-20 19:20:27.703')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'148', N'1', N'1', N'오늘은 다블이 꿈★', N'[선물목록]의 열쇠 받고, 잠들기 전 다블 한판! 어떠세요~?', N'3', N'9/21, 21시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-09-20 19:21:37.960')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'149', N'1', N'1', N'선물 도착했어요~!', N'상큼한 목요일 아침! 다블 접속하셔서 [선물목록] 확인하세요~', N'3', N'9/22, 8시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-09-21 23:07:54.390')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'150', N'1', N'1', N'배고픔을 채워줄 열쇠 선물~', N'벌써 점심이에요~ 맛점하시구 [선물목록]에 도착한 열쇠 선물 확인하세요!', N'3', N'9/22, 12시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-09-21 23:09:40.940')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'151', N'1', N'1', N'6시 종이 땡땡땡~', N'6시 종과 함께 도착한 열쇠 선물! [선물목록]에서 확인하세요~', N'3', N'9/22, 18시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-09-21 23:10:33.727')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'152', N'1', N'1', N'선물이 도착했습니다~!', N'신나는 목요일 밤, 다블을 잊고 계신건 아니죠..? 신나는 이 시간도 역시 다블과 함께!', N'3', N'9/22, 21시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-09-21 23:12:03.250')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'153', N'1', N'1', N'가을의 시작을 알리며...', N'맑은 가을 하늘 한번 바라보고, 다블 한판 하시라고 [선물목록]에 열쇠 준비해두었답니다~!', N'3', N'9/23, 8시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-09-22 20:36:16.730')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'154', N'1', N'1', N'점심 열쇠 도착!', N'어김없이 찾아온 다블의 열쇠 선물! 게임 내 [선물목록]에서 확인하세요~', N'3', N'9/23, 12시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-09-22 20:37:17.390')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'155', N'1', N'1', N'불금의 시작은 다블과 함께!', N'[선물목록]에서 열쇠 받고, 불금을 신나게 불태워보세요~', N'3', N'9/23, 18시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-09-22 20:38:14.400')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'156', N'1', N'1', N'이 밤의 끝을 잡고..', N'이 밤이 지나가기 전에 다블 한판 더 즐기시라고 열쇠선물 보내드렸습니다~', N'3', N'9/23, 21시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-09-22 20:39:04.417')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'157', N'1', N'1', N'선물목록에 열쇠가 도착했습니다', N'다크블레이즈에서 [선물목록] 확인해주세요~', N'3', N'9/24, 8시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-09-23 15:01:17.723')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'158', N'1', N'1', N'점심 선물이 도착했습니다~', N'다들 맛점 하고 계신가요~? 다블에서 열쇠를 보내드렸습니다~', N'3', N'9/24, 12시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-09-23 15:02:03.203')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'159', N'1', N'1', N'저녁 먹기 전 다블 한판!', N'[선물목록]의 열쇠 확인하시고 다블을 즐겨보세요~', N'3', N'9/24, 18시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-09-23 15:03:17.607')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'160', N'1', N'1', N'열쇠 부족하시죠..?', N'열심히 다블을 즐기시는 영웅님을 위해 [선물목록]에 열쇠 넣어두었습니다~', N'3', N'9/24, 21시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-09-23 15:03:50.717')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'161', N'1', N'1', N'일요일 첫 번째 열쇠 도착!', N'다블이가 잊지않고 [선물목록]에 열쇠 넣어두었답니다!', N'3', N'9/25, 8시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-09-23 15:05:00.600')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'162', N'1', N'1', N'열쇠 선물이 도착했습니다.', N'열쇠 받고 보스를 한번 물리쳐 보는건 어떨까요?', N'3', N'9/25, 12시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-09-23 15:06:32.823')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'163', N'1', N'1', N'[다블] 선물로 열쇠드려요~', N'열쇠 받고 오랜만에 수동전투 한판! 어떠세요~?', N'3', N'9/25, 18시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-09-23 15:08:48.150')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'164', N'1', N'1', N'다크블레이즈 선물 도착!', N'[선물 목록]에 열쇠가 들어왔습니다. 받아..주실거죠..?', N'3', N'9/25, 21시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-09-23 15:10:19.850')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'165', N'1', N'1', N'출근길 열쇠 도착!', N'무거운 출근길, 조금이라도 가벼워지시라고 열쇠 보내드립니다!', N'3', N'9/26, 8시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-09-23 15:11:40.837')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'166', N'1', N'1', N'[선물목록] 열쇠 80개 도착', N'열쇠 80개 받으시고 다크블레이즈 전투를 즐겨보세요~', N'3', N'9/26, 12시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-09-23 15:12:41.477')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'167', N'1', N'1', N'다블이의 토닥토닥', N'열심히 일한 당신, 열쇠 받고 다크블레이즈 하라!', N'3', N'9/26, 18시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-09-23 15:13:39.300')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'168', N'1', N'1', N'다블이의 선물 도착!', N'[선물목록]에 열쇠 받으시고 편안한 밤 되세요~', N'3', N'9/26, 21시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-09-23 15:14:40.590')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'169', N'1', N'1', N'열쇠 60개, 선물 도착!', N'열쇠가 도착했습니다. 게임 내 [선물 목록]에서 수령 받고, 게임을 즐기세요~', N'3', N'9/27, 8시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-09-26 19:04:16.683')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'170', N'1', N'1', N'점심 먹고, 열쇠 받고!', N'점심 휴식 시간 ~ [선물 목록]에서 열쇠 받고, 다크블레이즈 한판!', N'3', N'9/27, 12시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-09-26 19:04:59.587')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'171', N'1', N'1', N'열쇠가 부족하다!', N'그렇다면 지금 바로 [선물 목록]에서 열쇠 받으세요~', N'3', N'9/27, 18시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-09-26 19:05:25.227')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'172', N'1', N'1', N'늦은 밤, 열쇠 도착!', N'[선물 목록]에 도착한 열쇠 챙기시고~ 게임도 즐기시고~', N'3', N'9/27, 21시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-09-26 19:05:48.403')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'173', N'1', N'1', N'상쾌한 아침! 열쇠 60개!', N'아침 선물로 열쇠 60개 도착! [선물 목록]에서 수령 받으세요~', N'3', N'9/28, 8시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-09-27 18:07:09.687')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'174', N'1', N'1', N'맛점 하세요~ 열쇠는 선물~', N'[선물 목록]에 열쇠가 도착했습니다~', N'3', N'9/28, 12시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-09-27 18:07:34.333')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'175', N'1', N'1', N'열쇠 60개, 선물 도착!', N'잊지 말고 [선물 목록]에서 열쇠 수령 받으시고, 던전으로 고고고~', N'3', N'9/28, 18시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-09-27 18:07:59.800')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'176', N'1', N'1', N'오늘의 마지막 열쇠!', N'[선물 목록]에 도착한 열쇠 챙기시고~ 게임도 즐기시고~', N'3', N'9/28, 21시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-09-27 18:09:08.903')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'177', N'1', N'1', N'출근길 선물 도착~', N'출근길 선물 도착~ [선물 목록]에 도착한 열쇠 받으세요~', N'3', N'9/29, 8시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-09-28 20:06:52.863')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'178', N'1', N'1', N'점심 시간! 다크블레이즈 한판', N'[선물 목록]에서 열쇠 받고, 다크블레이즈 한판!', N'0', N'9/29, 12시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-09-28 20:07:22.297')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'179', N'1', N'1', N'퇴근길 열쇠 선물 도착!', N'퇴근길 무료하지 않게, [선물 목록]에 열쇠 60개 도착~', N'3', N'9/29, 18시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-09-28 20:08:05.527')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'180', N'1', N'1', N'열~쇠 받아요~!', N'오늘 하루 마무리 전, [선물 목록]에서 열쇠 받고 다크블레이즈 한판~', N'3', N'9/29, 21시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-09-28 20:08:23.223')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'181', N'1', N'1', N'점검 끝! 다크블레이즈 한판!', N'[선물 목록]에서 열쇠 받고, 다크블레이즈 한판!', N'0', N'9/29, 12시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-09-29 11:38:39.487')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'182', N'1', N'1', N'출근길도 다크블레이즈와 함께!', N'출근길에 [선물목록]에서 열쇠 받아가세요~', N'3', N'9/30, 8시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-09-29 21:07:03.793')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'183', N'1', N'1', N'점심 시간! 다크블레이즈 한판', N'점심 휴식 시간 ~ [선물 목록]에서 열쇠 받고, 다크블레이즈 한판!', N'3', N'9/30, 12시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-09-29 21:07:26.620')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'184', N'1', N'1', N'칼퇴하고 다크블레이즈 한판!', N'영웅님들의 퇴근길을 책임질 다크블레이즈! [선물 목록]에서 열쇠 받아가세요!', N'3', N'9/30, 18시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-09-29 21:07:43.283')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'185', N'1', N'1', N'열~쇠 받아요~!', N'오늘 하루 마무리 전, [선물 목록]에서 열쇠 받고 다크블레이즈 한판~', N'3', N'9/30, 21시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-09-29 21:08:01.190')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'186', N'1', N'1', N'토요일 아침 꿀모닝 보상', N'꿀잠자고 다크블레이즈 한판! 열쇠 보상 받아가세요~', N'3', N'10/1, 8시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-09-30 17:02:03.910')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'187', N'1', N'1', N'다크블레이즈와 함께하는 오후', N'아점 먹고 나른한 오후 다크블레이즈 한판! [선물 목록]에 열쇠가 도착했어요!', N'3', N'10/1, 12시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-09-30 17:02:31.263')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'188', N'1', N'1', N'불타는 토요일은? 다블과 함께', N'지금 접속하시면 열쇠 선물이 팡! [선물 목록]고고씽!', N'3', N'10/1, 18시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-09-30 17:03:02.470')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'189', N'1', N'1', N'토요일밤에~ 다블 한판해~♬', N'[선물 목록]에 가면 열쇠 선물이 와르르~~~!!', N'3', N'10/1, 21시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-09-30 17:03:23.927')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'190', N'1', N'1', N'좋은 아침! 선물 왔어요~', N'졸린 눈 비비며 [선물목록]의 열쇠 받아서 다블하며 잠을 깨 보아요~♥', N'3', N'10/2, 8시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-09-30 17:03:45.910')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'191', N'1', N'1', N'선물목록에 뭔가 도착했어요~', N'짜잔~! [선물목록]에 열쇠가 도착했습니다~ 신나게 다블 한판 고고씽~!', N'3', N'10/2, 12시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-09-30 17:04:02.980')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'192', N'1', N'1', N'띠리링! 선물이 도착했습니다.', N'열쇠 부족하셨죠? [선물목록]에 열쇠가 도착했습니다~', N'3', N'10/2, 18시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-09-30 17:04:21.190')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'193', N'1', N'1', N'열쇠가 왔습니다~', N'[선물목록]에 열쇠가 도착했습니다~ 선물 받고 편안한 밤 되세요~', N'3', N'10/2, 21시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-09-30 17:04:57.367')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'194', N'1', N'1', N'열쇠가~ 도착했다!', N'[선물 목록]에 열쇠가 도착했습니다~', N'3', N'10/3, 8시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-09-30 17:05:28.983')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'195', N'1', N'1', N'점심 시간! 다크블레이즈 한판', N'점심 휴식 시간 ~ [선물 목록]에서 열쇠 받고, 다크블레이즈 한판!', N'3', N'10/3, 12시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-09-30 17:05:46.717')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'196', N'1', N'1', N'열쇠 60개, 선물 도착!', N'[선물 목록]에서 열쇠 꼭 챙기세요 ~', N'3', N'10/3, 18시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-09-30 17:06:06.313')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'197', N'1', N'1', N'늦은 밤, 열쇠 도착!', N'아쉬운 휴일 저녁, [선물 목록]에서 열쇠 받고 다크블레이즈 한판 즐기시죠~', N'3', N'10/3, 21시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-09-30 17:06:26.470')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'198', N'1', N'1', N'오늘도 힘차게!', N'출근길 선물 도착~ [선물 목록]에 도착한 열쇠 받으세요~', N'3', N'10/4, 8시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-09-30 17:06:47.110')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'199', N'1', N'1', N'맛점 하세요~ 열쇠는 선물~', N'점심 휴식 시간 ~ [선물 목록]에서 열쇠 받고, 다크블레이즈 한판!', N'0', N'10/4, 12시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-09-30 17:07:05.000')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'200', N'1', N'1', N'퇴근길 열쇠 선물 도착!', N'열쇠가 도착했습니다. 게임 내 [선물 목록]에서 수령 받고, 던전으로 고고고!', N'3', N'10/4, 18시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-09-30 17:07:21.270')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'201', N'1', N'1', N'열~쇠 받아요~!', N'오늘 하루 마무리 전, [선물 목록]에서 열쇠 받고 다크블레이즈 한판~', N'3', N'10/4, 21시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-09-30 17:07:49.687')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'202', N'1', N'1', N'맛점 하세요~ 열쇠는 선물~', N'점심 휴식 시간 ~ [선물 목록]에서 열쇠 받고, 다크블레이즈 한판!', N'3', N'10/4, 12시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-09-30 17:08:21.847')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'203', N'1', N'1', N'오늘도 힘차게!', N'출근길 선물 도착~ [선물 목록]에 도착한 열쇠 받으세요~', N'3', N'10/5, 8시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-10-04 13:04:43.500')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'204', N'1', N'1', N'맛점 하세요~ 열쇠는 선물~', N'점심 휴식 시간 ~ [선물 목록]에서 열쇠 받고, 다크블레이즈 한판!', N'3', N'10/5, 12시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-10-04 13:04:59.400')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'205', N'1', N'1', N'퇴근길 열쇠 선물 도착!', N'열쇠가 도착했습니다. 게임 내 [선물 목록]에서 수령 받고, 던전으로 고고고!', N'3', N'10/5, 18시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-10-04 13:05:22.570')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'206', N'1', N'1', N'오늘의 마지막 열쇠!', N'잊지 말고 [선물 목록]에서 열쇠 수령 받으시고, 던전으로 고고고~', N'3', N'10/5, 21시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-10-04 13:05:44.320')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'207', N'1', N'1', N'출근길 선물 도착~', N'[선물 목록]에 열쇠가 도착했습니다. 열쇠 받고 다블 한판 어떠세요~?', N'3', N'10/6, 8시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-10-05 11:14:02.460')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'208', N'1', N'1', N'열쇠가~ 도착했다!', N'[선물 목록]에 도착한 열쇠 챙기시고~ 게임도 즐기시고~', N'3', N'10/6, 12시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-10-05 11:14:28.333')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'209', N'1', N'1', N'열쇠 60개, 선물 도착!', N'열쇠가 도착했습니다. 게임 내 [선물 목록]에서 수령 받고, 게임을 즐기세요.', N'3', N'10/6, 18시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-10-05 11:14:54.770')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'210', N'1', N'1', N'열쇠 도착!', N'열쇠가 도착했습니다. 게임 내 [선물 목록]에서 수령 받으세요~', N'3', N'10/6, 21시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-10-05 11:15:21.477')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'211', N'1', N'1', N'선물목록에 열쇠가 도착했습니다', N'다크블레이즈에서 [선물목록] 확인해주세요~', N'3', N'10/7, 8시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-10-06 15:53:15.000')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'212', N'1', N'1', N'점심 열쇠 도착!', N'어김없이 찾아온 다블의 열쇠 선물! 게임 내 [선물목록]에서 확인하세요~', N'3', N'10/7, 12시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-10-06 15:53:33.497')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'213', N'1', N'1', N'불금의 시작은 다블과 함께!', N'[선물목록]에서 열쇠 받고, 불금을 신나게 불태워보세요~', N'3', N'10/7, 18시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-10-06 15:53:49.657')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'214', N'1', N'1', N'이 밤의 끝을 잡고..', N'이 밤이 지나가기 전에 다블 한판 더 즐기시라고 열쇠선물 보내드렸습니다~', N'3', N'10/7, 21시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-10-06 15:54:21.993')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'215', N'1', N'1', N'상쾌한 아침! 열쇠 60개!', N'아침 선물로 열쇠 60개 도착! [선물 목록]에서 수령 받으세요~', N'3', N'10/8, 8시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-10-07 10:14:52.527')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'216', N'1', N'1', N'맛점 하세요~ 열쇠는 선물~', N'[선물 목록]에 열쇠가 도착했습니다~', N'3', N'10/8, 12시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-10-07 10:16:11.573')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'217', N'1', N'1', N'열쇠가 부족하다!', N'그렇다면 지금 바로 [선물 목록]에서 열쇠 받으세요~', N'3', N'10/8, 18시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-10-07 10:16:33.350')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'218', N'1', N'1', N'늦은 밤, 열쇠 도착!', N'[선물 목록]에 도착한 열쇠 챙기시고~ 게임도 즐기시고~', N'3', N'10/8, 21시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-10-07 10:16:53.980')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'219', N'1', N'1', N'굿모닝~! 열쇠 받으세요~', N'[선물 목록]에서 열쇠 받고 즐거운 하루 보내세요~!', N'3', N'10/9, 8시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-10-07 10:17:31.950')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'220', N'1', N'1', N'우와~! 벌써 점심시간~!', N'맛점하시구 다크블레이즈 한판! [선물 목록]에서 열쇠 받아가세요~!', N'3', N'10/9, 12시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-10-07 10:17:49.853')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'221', N'1', N'1', N'열~쇠 받아요~!', N'[선물 목록]에 열쇠가 도착했습니다~', N'3', N'10/9, 18시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-10-07 10:18:51.230')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'222', N'1', N'1', N'휴일 끝자락 열쇠 선물!', N'[선물 목록]에 도착한 열쇠 미리 미리 챙기세요~', N'3', N'10/9, 21시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-10-07 10:19:13.657')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'223', N'1', N'1', N'출근길 선물 도착~', N'출근길 선물 도착~ [선물 목록]에 도착한 열쇠 받으세요~', N'3', N'10/10, 8시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-10-07 10:32:30.733')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'224', N'1', N'1', N'점심 시간! 다크블레이즈 한판', N'점심 휴식 시간 ~ [선물 목록]에서 열쇠 받고, 다크블레이즈 한판!', N'3', N'10/10, 12시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-10-07 10:32:50.810')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'225', N'1', N'1', N'퇴근길 열쇠 선물 도착!', N'퇴근길 무료하지 않게, [선물 목록]에 열쇠 60개 도착~', N'3', N'10/10, 18시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-10-07 10:33:07.780')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'226', N'1', N'1', N'열~쇠 받아요~!', N'오늘 하루 마무리 전, [선물 목록]에서 열쇠 받고 다크블레이즈 한판~', N'3', N'10/10, 21시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-10-07 10:33:31.330')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'227', N'1', N'1', N'상쾌한 아침에 다블 한판!', N'다크블레이즈 접속하셔서 [선물 목록]을 보면 열쇠 선물이 짠!!', N'3', N'10/11, 8시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-10-07 10:34:44.240')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'228', N'1', N'1', N'맛점하고 선물 받기~', N'맛있는 점심 드셨나요~? 다블이도 배부르게 [선물목록]에서 열쇠 받아주세요~^^', N'3', N'10/11, 12시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-10-07 10:35:11.573')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'229', N'1', N'1', N'다블이와 저녁 만찬!', N'[선물목록] 확인하시고 푸짐한 저녁 드세요~!', N'3', N'10/11, 18시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-10-07 10:35:38.487')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'230', N'1', N'1', N'오늘도 너무 수고하신 영웅님께', N'다블이가 드리는 열쇠 선물이 도착! 수고하셨습니다♥', N'3', N'10/11, 21시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-10-07 10:35:56.820')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'231', N'1', N'1', N'열쇠 도착!', N'열쇠가 도착했습니다. 게임 내 [선물 목록]에서 수령 받으세요~', N'3', N'10/12, 8시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-10-07 10:37:59.737')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'232', N'1', N'1', N'점심 열쇠 도착!', N'어김없이 찾아온 다블의 열쇠 선물! 게임 내 [선물목록]에서 확인하세요~', N'3', N'10/12, 12시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-10-07 10:38:19.703')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'233', N'1', N'1', N'퇴근길 열쇠 선물 도착!', N'열쇠가 도착했습니다. 게임 내 [선물 목록]에서 수령 받고, 던전으로 고고고!', N'3', N'10/12, 18시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-10-07 10:38:59.577')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'234', N'1', N'1', N'오늘의 마지막 열쇠!', N'잊지 말고 [선물 목록]에서 열쇠 수령 받으시고, 던전으로 고고고~', N'3', N'10/12, 21시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-10-07 10:39:28.937')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'235', N'1', N'1', N'오늘도 힘차게!', N'출근길 선물 도착~ [선물 목록]에 도착한 열쇠 받으세요~', N'3', N'10/13, 8시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-10-07 10:40:19.497')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'236', N'1', N'1', N'맛점 하세요~ 열쇠는 선물~', N'점심 휴식 시간 ~ [선물 목록]에서 열쇠 받고, 다크블레이즈 한판!', N'3', N'10/13, 12시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-10-07 10:40:38.353')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'237', N'1', N'1', N'퇴근길 열쇠 선물 도착!', N'[선물 목록]에서 열쇠 꼭 챙기세요 ~', N'3', N'10/13, 18시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-10-07 10:41:09.760')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'238', N'1', N'1', N'늦은 밤, 열쇠 도착!', N'오늘 하루 마무리 전, [선물 목록]에서 열쇠 받고 다크블레이즈 한판~', N'3', N'10/13, 21시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-10-07 10:41:34.730')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'239', N'1', N'1', N'선물 도착했어요~!', N'상큼한 금요일 아침! 다블 접속하셔서 [선물목록] 확인하세요~', N'3', N'10/14, 8시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-10-07 10:42:55.837')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'240', N'1', N'1', N'배고픔을 채워줄 열쇠 선물~', N'벌써 점심이에요~ 맛점하시구 [선물목록]에 도착한 열쇠 선물 확인하세요!', N'3', N'10/14, 12시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-10-07 10:43:16.900')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'241', N'1', N'1', N'불금의 시작은 다블과 함께!', N'[선물목록]에서 열쇠 받고, 불금을 신나게 불태워보세요~', N'3', N'10/14, 18시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-10-07 10:43:35.483')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'242', N'1', N'1', N'선물이 도착했습니다~!', N'신나는 금요일 밤, 다블을 잊고 계신건 아니죠..? 신나는 이 시간도 역시 다블과 함께!', N'3', N'10/14, 21시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-10-07 10:44:01.707')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'243', N'1', N'1', N'선물목록에 열쇠가 도착했습니다', N'다크블레이즈에서 [선물목록] 확인해주세요~', N'3', N'10/15, 8시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-10-11 18:48:45.417')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'244', N'1', N'1', N'점심 선물이 도착했습니다~', N'다들 맛점 하고 계신가요~? 다블에서 열쇠를 보내드렸습니다~', N'3', N'10/15, 12시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-10-11 18:49:00.260')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'245', N'1', N'1', N'저녁 먹기 전 다블 한판!', N'[선물목록]의 열쇠 확인하시고 다블을 즐겨보세요~', N'3', N'10/15, 18시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-10-11 18:49:14.307')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'246', N'1', N'1', N'열쇠 부족하시죠..?', N'열심히 다블을 즐기시는 영웅님을 위해 [선물목록]에 열쇠 넣어두었습니다~', N'3', N'10/15, 21시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-10-11 18:49:30.300')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'247', N'1', N'1', N'열쇠 60개가 짠~!', N'다크블레이즈 들어오셔서 [선물목록]을 확인하면? 열쇠가 짠~! 들어왔답니다~^^', N'3', N'10/16, 8시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-10-11 18:50:26.187')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'248', N'1', N'1', N'맛점하시고 다블 한판!', N'[선물목록]의 열쇠 받으시고 다블과 함께 맛점하세요~!', N'3', N'10/16, 12시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-10-11 18:50:42.757')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'249', N'1', N'1', N'다크블레이즈 열쇠 도착!', N'[선물 목록]에 열쇠가 도착했습니다. 열쇠 받고 다블 한판 어떠세요~?', N'3', N'10/16, 18시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-10-11 18:50:56.997')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'250', N'1', N'1', N'열쇠 받고 오늘의 마무리~!', N'다크블레이즈에서 열쇠를 보냈답니다~ [선물목록]에서 확인해주세요~^^', N'3', N'10/16, 21시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-10-11 18:51:22.363')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'251', N'1', N'1', N'출근길 열쇠 도착!', N'무거운 출근길, 조금이라도 가벼워지시라고 열쇠 보내드립니다!', N'3', N'10/17, 8시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-10-11 18:52:24.437')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'252', N'1', N'1', N'[선물목록] 열쇠 80개 도착', N'열쇠 80개 받으시고 다크블레이즈 전투를 즐겨보세요~', N'3', N'10/17, 12시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-10-11 18:52:42.253')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'253', N'1', N'1', N'다블이의 토닥토닥', N'열심히 일한 당신, 열쇠 받고 다크블레이즈 하라!', N'3', N'10/17, 18시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-10-11 18:52:58.413')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'254', N'1', N'1', N'다블이의 선물 도착!', N'[선물목록]에 열쇠 받으시고 편안한 밤 되세요~', N'3', N'10/17, 21시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-10-11 18:53:15.127')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'255', N'1', N'1', N'출근길 선물 도착~', N'출근길 선물 도착~ [선물 목록]에 도착한 열쇠 선물 받으세요~', N'3', N'10/18, 8시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-10-17 15:56:24.383')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'256', N'1', N'1', N'점심 시간! 다크블레이즈 한판', N'[선물 목록]에서 열쇠 받고, 다크블레이즈 한판!', N'3', N'10/18, 12시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-10-17 15:56:42.380')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'257', N'1', N'1', N'퇴근길 열쇠 선물 도착!', N'퇴근길 무료하지 않게, [선물 목록]에 열쇠 60개 도착~', N'3', N'10/18, 18시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-10-17 15:57:08.960')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'258', N'1', N'1', N'열쇠 받고 오늘의 마무리~!', N'다크블레이즈에서 열쇠를 보냈답니다~ [선물목록]에서 확인해주세요~^^', N'3', N'10/18, 21시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-10-17 15:57:47.483')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'259', N'1', N'1', N'열쇠 60개가 짠~!', N'다크블레이즈 들어오셔서 [선물목록]을 확인하면? 열쇠가 짠~! 들어왔답니다~^^', N'3', N'10/19, 8시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-10-17 15:58:54.133')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'260', N'1', N'1', N'맛점 하세요~ 열쇠는 선물~', N'[선물 목록]에 열쇠가 도착했습니다~', N'3', N'10/19, 12시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-10-17 15:59:19.400')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'261', N'1', N'1', N'열쇠 선물 도착!', N'잊지 말고 [선물 목록]에서 열쇠 수령 받으시고, 던전으로 고고고~', N'3', N'10/19, 18시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-10-17 15:59:39.673')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'262', N'1', N'1', N'오늘의 마지막 열쇠!', N'[선물 목록]에 도착한 열쇠 챙기시고~ 게임도 즐기세요~', N'3', N'10/19, 21시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-10-17 16:00:00.500')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'263', N'1', N'1', N'좋은 아침! 선물 왔어요~', N'짜잔~! [선물목록]에 열쇠가 도착했습니다~ 신나게 다블 한판 고고씽~!', N'3', N'10/20, 8시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-10-17 16:00:52.203')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'264', N'1', N'1', N'점심 시간! 다크블레이즈 한판', N'점심 휴식 시간 ~ [선물 목록]에서 열쇠 받고, 다크블레이즈 한판!', N'3', N'10/20, 12시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-10-17 16:01:19.590')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'265', N'1', N'1', N'열쇠가 왔습니다~', N'열쇠 부족하셨죠? [선물목록]에 열쇠가 도착했습니다~', N'3', N'10/20, 18시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-10-17 16:01:50.490')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'266', N'1', N'1', N'늦은 밤, 열쇠 도착!', N'[선물목록]에 열쇠가 도착했습니다~ 선물 받고 편안한 밤 되세요~', N'3', N'10/20, 21시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-10-17 16:02:14.380')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'267', N'1', N'1', N'출근길 선물 도착~', N'[선물 목록]에 열쇠가 도착했습니다. 열쇠 받고 다블 한판 어떠세요~?', N'3', N'10/21, 8시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-10-17 16:02:53.677')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'268', N'1', N'1', N'점심 열쇠 도착!', N'어김없이 찾아온 다블의 열쇠 선물! 게임 내 [선물목록]에서 확인하세요~', N'3', N'10/21, 12시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-10-17 16:03:12.600')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'269', N'1', N'1', N'선물목록에 열쇠가 도착했습니다', N'다크블레이즈에서 [선물목록] 확인해주세요~', N'3', N'10/21, 18시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-10-17 16:03:28.937')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'270', N'1', N'1', N'열~쇠 받아요~!', N'[선물 목록]에 열쇠가 도착했습니다~', N'3', N'10/21, 21시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-10-17 16:03:54.600')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'271', N'1', N'1', N'상쾌한 아침! 열쇠 도착~!', N'아침 선물로 열쇠 60개 도착! [선물 목록]에서 수령 받으세요~', N'3', N'10/22, 8시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-10-17 16:04:32.593')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'272', N'1', N'1', N'맛점하시고 다블 한판!', N'[선물목록]의 열쇠 받으시고 다블과 함께 맛점하세요~!', N'3', N'10/22, 12시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-10-17 16:04:53.000')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'273', N'1', N'1', N'다크블레이즈 열쇠 도착!', N'[선물 목록]에 열쇠가 도착했습니다. 열쇠 받고 다블 한판 어떠세요~?', N'3', N'10/22, 18시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-10-17 16:05:10.343')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'274', N'1', N'1', N'선물목록에 뭔가 도착했어요~', N'짜잔~! [선물목록]에 열쇠가 도착했습니다~ 신나게 다블 한판 고고씽~!', N'3', N'10/22, 21시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-10-17 16:05:41.823')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'275', N'1', N'1', N'띠리링! 선물이 도착했습니다', N'열쇠 부족하셨죠? [선물목록]에 열쇠가 도착했습니다~', N'3', N'10/23, 8시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-10-17 16:06:20.480')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'276', N'1', N'1', N'배고픔을 채워줄 열쇠 선물~', N'벌써 점심이에요~ 맛점하시구 [선물목록]에 도착한 열쇠 선물 확인하세요!', N'3', N'10/23, 12시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-10-17 16:06:41.457')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'277', N'1', N'1', N'6시 종이 땡땡땡~', N'6시 종과 함께 도착한 열쇠 선물! [선물목록]에서 확인하세요~', N'3', N'10/23, 18시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-10-17 16:06:59.540')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'278', N'1', N'1', N'[선물목록] 열쇠 도착!', N'지금 다크블레이즈 [선물목록]에 열쇠가 도착했답니다~ 늦지 않게 꼭 받아가세요~!', N'3', N'10/23, 21시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-10-17 16:07:20.523')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'279', N'1', N'1', N'좋은 아침! 선물 왔어요~', N'졸린 눈 비비며 [선물목록]의 열쇠 받아서 다블하며 잠을 깨 보아요~♥', N'3', N'10/24, 8시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-10-17 16:07:55.473')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'280', N'1', N'1', N'열쇠 받을 시간이에요~', N'열쇠 받는 것 잊지 않으셨죠~? 다크블레이즈 [선물목록]에서 열쇠 받아가세요~', N'3', N'10/24, 12시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-10-17 16:08:18.893')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'281', N'1', N'1', N'열쇠가 부족하다!', N'그렇다면 지금 바로 [선물 목록]에서 열쇠 받으세요~', N'3', N'10/24, 18시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-10-17 16:08:46.323')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'282', N'1', N'1', N'늦은 밤, 열쇠 도착!', N'[선물 목록]에 도착한 열쇠 챙기시고~ 게임도 즐기시고~', N'3', N'10/24, 21시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-10-17 16:09:02.893')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'283', N'1', N'1', N'회사가는길, 선물받아가세요~', N'열쇠는 [선물 목록]에 있습니다~ 잊지말고 챙겨가세요~', N'3', N'10/25, 8시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-10-21 16:05:23.430')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'284', N'1', N'1', N'점심먹고 게임 한판! ', N'나른한 오후, 다크블레이즈 [선물목록]에서 열쇠 받고 게임 한판 GoGo!', N'3', N'10/26, 12시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-10-21 16:11:16.213')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'285', N'1', N'0', N'집가는 길,열쇠받고 게임하자!', N'다크블레이즈 [선물목록]에서 열쇠 받아가세요~~', N'0', N'10/25, 18시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-10-21 16:13:37.110')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'286', N'1', N'1', N'자기 전에 열쇠받아가세요~', N'열쇠가 부족하셨나요? [선물목록]에서 열쇠 받아가세요~', N'3', N'10/25, 21시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-10-21 16:16:33.913')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'287', N'1', N'1', N'점심먹고 게임 한판!', N'나른한 오후, 다크블레이즈 [선물목록]에서 열쇠 받고 게임 한판 GoGo!', N'0', N'10/25, 12시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-10-21 16:18:48.133')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'288', N'1', N'1', N'집가는 길,열쇠받고 게임하자!', N'다크블레이즈 [선물목록]에서 열쇠 받아가세요~~', N'0', N'10/25, 18시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-10-21 16:19:27.130')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'289', N'1', N'1', N'눈비비고 일어나 선물받으세요~', N'열쇠가 부족하셨나요? 다크블레이즈[선물목록]에서 열쇠 받아 가세요~', N'3', N'10/26, 8시푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-10-21 16:23:08.553')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'290', N'1', N'1', N'열쇠받은 시간이 돌아왔습니다~', N'부족한 열쇠 채우시는거 잊지 않으셨요? [선물목록]에서 열쇠 챙기세요~', N'3', N'10/26, 12시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-10-21 16:27:08.357')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'291', N'1', N'1', N'다크블레이즈 열쇠 도착!', N'열쇠가 도착했습니다. 열쇠 받고 다크블레이즈 한판 어떠세요~?', N'3', N'10/26, 18시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-10-21 16:28:26.340')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'292', N'1', N'1', N'다크블레이즈 열쇠선물입니다~!', N'지금 다크블레이즈 [선물목록]에 열쇠가 도착했습니다~ 늦지 않게 꼭 받아가세요~!', N'3', N'10/26, 21시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-10-21 16:30:10.150')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'293', N'1', N'1', N'잠에서 일어나 다블 한판~!', N'[선물목록]에서 열쇠 받아가세요~', N'3', N'10/27, 8시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-10-21 16:49:07.807')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'294', N'1', N'1', N'열쇠받고 다크블레이즈 한판~!', N'맛점하시고 [선물목록]에서 열쇠 받아가는거 잊지마세용~', N'3', N'10/27, 12시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-10-21 16:51:11.470')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'295', N'1', N'1', N'열쇠가 부족하셨나요??', N'[선물목록]에서 열쇠 챙기시고, 다크블레이즈하며 스트레스를 날리세요~', N'0', N'10/27, 18시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-10-21 16:52:48.943')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'296', N'1', N'1', N'열!쇠! 받는!! 시간!!', N'다크블레이즈 [선물목록]에서 열쇠 챙기고 즐겜하세요~!', N'3', N'10/27, 21시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-10-21 16:54:41.113')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'297', N'1', N'1', N'열쇠 잊지마세요~', N'금요일 아침! 다크블레이즈하며 조금만 더 힘내세요~!!', N'3', N'10/28, 8시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-10-21 16:57:37.890')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'298', N'1', N'1', N'점심먹고 다크블레이즈 하자~', N'다크블레이즈 [선물목록]에서 열쇠 챙기세요~!!', N'3', N'10/28, 12시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-10-21 16:59:09.673')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'299', N'1', N'1', N'열쇠받고 불금은 시작된다!!', N'불금을 계획하시나요? 다블 열쇠 받는거 잊지마시고 게임을 즐기세요~', N'3', N'10/28, 18시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-10-21 17:01:00.693')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'300', N'1', N'1', N'열쇠부족하신가요?', N'다크블레이즈 [선물목록]에서 무료  열쇠 확인하세요', N'3', N'10/28, 21시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-10-21 17:03:03.630')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'301', N'1', N'1', N'토요일 오전 열쇠선물입니다~~', N'주말 오전, 다크블레이즈하며 활기찬 하루 보내세요~~', N'3', N'10/29, 8시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-10-21 17:07:36.467')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'302', N'1', N'1', N'선물이 도착했습니다!', N'[선물목록]에 열쇠 있는거 아시죠~? 잊지마시고 꼭 챙겨가세요!', N'3', N'10/29, 12시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-10-21 17:10:40.347')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'303', N'1', N'1', N'다블하며 스트레스 날려!날려!', N'즐거운 퇴근시간~~ [선물목록]에서 열쇠 받아가세요~', N'3', N'10/29, 18시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-10-21 17:13:46.723')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'304', N'1', N'1', N'다블이와 함께 꿀잠♥', N'지금 열쇠 받으시고 자기 전 다블 한판! [선물 목록]을 확인해 주세요^^', N'3', N'10/29, 21시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-10-21 17:15:04.697')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'305', N'1', N'1', N'혹시...열쇠 부족하신가요?', N'부족한 열쇠는 채우고! 다블하며 스트레스도 치우세요!', N'3', N'10/30, 8시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-10-21 17:22:23.727')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'306', N'1', N'1', N'열쇠 선물입니다~★', N'나른한 주말의 오후, 식사 맛있게 하시고 다크블레이즈하며 즐거움을 느끼세요~', N'3', N'10/30, 12시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-10-21 17:23:53.760')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'307', N'1', N'1', N'열쇠 챙기세용~', N'밥먹기 전에 열쇠 챙기세용~ 다크블레이즈 [선물목록]에서 확인가능 합니다~^^', N'3', N'10/30, 18시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-10-21 17:25:21.320')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'308', N'1', N'1', N'열쇠~선물입니다~~', N'지나가는 주말이 아쉽다면 다크블레이즈를 해보세요~ 열쇠는[선물목록]에 있습니다^^', N'3', N'10/30 ,21시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-10-21 17:27:53.907')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'309', N'1', N'0', N'열쇠 60개, 선물 도착!', N'[선물 목록]에서 열쇠 꼭 챙기세요 ~', N'0', N'10/31, 8시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-10-21 17:30:33.920')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'310', N'1', N'1', N'점심 선물이 도착했습니다~', N'다들 맛점 하고 계신가요~? 다블에서 열쇠를 보내드렸습니다~', N'3', N'10/31, 12시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-10-21 17:31:19.880')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'311', N'1', N'0', N'선물목록에 열쇠가 도착했습니다', N'월요일 출근이 힘드셨나요? 열쇠 받고 다크블레이즈하며 스트레스를 날리세요~~!', N'0', N'10/31, 18시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-10-21 17:33:14.997')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'312', N'1', N'1', N'10월의 마지막 열쇠^^', N'벌써 10월의 마지막 날이네요~ [선물목록]에서 열쇠 챙기세요~', N'3', N'10/31, 21시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-10-21 17:36:00.437')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'313', N'1', N'1', N'열쇠 60개, 선물 도착!', N'[선물 목록]에서 열쇠 꼭 챙기세요 ~	', N'3', N'10/31, 8시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-10-21 17:36:47.830')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'314', N'1', N'1', N'선물목록에 열쇠가 도착했습니다', N'월요일 출근이 힘드셨나요? 열쇠 받고 다크블레이즈하며 스트레스를 날리세요~~!', N'3', N'10/31, 18시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-10-21 17:37:22.837')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'315', N'1', N'1', N'점심 먹고, 열쇠 받고!', N'[선물 목록]에서 열쇠 받고, 다크블레이즈 한판하세요~!', N'3', N'10/25, 12시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-10-24 10:05:26.880')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'316', N'1', N'1', N'열쇠가 부족하셨나요??', N'[선물목록]에서 열쇠 챙기시고, 다크블레이즈하며 스트레스를 날리세요~', N'3', N'10/27, 18시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-10-24 10:22:06.540')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'317', N'1', N'1', N'굿모닝~! 열쇠왔어요~~', N'11월 첫날의 열쇠 80개 받아가세요~', N'0', N'11/1, 8시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-10-26 16:50:23.370')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'318', N'1', N'1', N'띠리링~ 선물도착했습니다.', N'[선물목록]에서 열쇠 찾아가세요~!', N'3', N'11/1, 12시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-10-26 16:51:46.580')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'319', N'1', N'1', N'퇴근하고 다크블레이즈 한판!', N'영웅님들의 퇴근길을 책임질 다크블레이즈! [선물 목록]에서 열쇠 받아가세요!', N'3', N'11/1, 18시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-10-26 16:52:59.830')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'320', N'1', N'1', N'다블하며 스트레스 날려!날려!', N'[선물목록]의 열쇠 챙기고 다크블레이즈 한판 어떠세요?', N'3', N'11/1, 21시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-10-26 16:55:52.467')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'321', N'1', N'1', N'굿모닝~! 열쇠왔어요~~', N'11월 첫날의 열쇠 선물을 받아가세요~', N'3', N'11/1, 8시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-10-27 10:04:53.300')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'322', N'1', N'1', N'일어나서 다블 한판 어떠세요?', N'스트레스 날리는 다블한판! [선물목록]에서 열쇠받아가세요~', N'3', N'11/2, 8시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-10-28 09:57:19.373')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'323', N'1', N'1', N'점심먹고 열쇠선물 잊지마세요!', N'즐거운 점심시간입니다~ 맞점하시고 열쇠챙기는거 잊지마세요~*^^*', N'0', N'11/2, 12시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-10-28 09:59:42.223')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'324', N'1', N'1', N'열쇠 도착!', N'열쇠가 도착했습니다. 게임 내 [선물 목록]에서 수령 받으세요~', N'3', N'11/2, 18시푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-10-28 13:06:54.530')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'325', N'1', N'1', N'오늘의 마지막 열쇠!', N'오늘의 마지막 열쇠선물을 잊지말고 챙기세요~', N'3', N'11/2, 21시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-10-28 13:07:49.910')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'326', N'1', N'1', N'출근길 선물이 도착했습니다~', N'출근하면서 다크블레이즈 한판하시는거 어떠신가요? [선물목록]에서 열쇠 확인하세요~', N'3', N'11/3, 8시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-10-28 13:10:12.437')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'327', N'1', N'1', N'점심먹고 다블하고', N'[선물목록]에 열쇠가 도착했습니다. 점심 맛있게 드세요^^', N'3', N'11/3, 12시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-10-28 13:11:23.530')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'328', N'1', N'1', N'띠리링~ 열쇠선물 왔습니다~', N'스트레스를 날리는 다크블레이즈! 열쇠 받고 다블도 즐기세요~', N'3', N'11/3, 18시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-10-28 13:12:57.137')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'329', N'1', N'1', N'오늘의 마지막 열쇠 도착!', N'하루의 마지막까지 책임지는 다크블레이즈!! [선물목록]에 열쇠가 도착했습니다~', N'3', N'11/3, 21시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-10-28 13:14:21.607')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'330', N'1', N'1', N'굿모닝입니다!!열쇠받으셔야죠?', N'매일 아침 열쇠선물이 도착하는거 아시죠? [선물목록]을 확인해보세요~', N'3', N'11/4, 8시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-10-28 13:16:38.450')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'331', N'1', N'1', N'[선물목록]을 확인해보세요.', N'스르르 잠들것 같은 오후, 다크블레이즈하며 잠을 깨는건 어떠세요?', N'3', N'11/4, 12시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-10-28 13:18:05.800')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'332', N'1', N'1', N'퇴근길에 열쇠선물 받아가세요~', N'11월 첫 불금이네요~!! 선물 받은 열쇠로 다크블레이즈를 즐겨보세요~', N'3', N'11/4, 18시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-10-28 13:20:18.860')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'333', N'1', N'1', N'열쇠왔습니다~', N'[선물목록]에서 열쇠 받아가세요~', N'3', N'11/4, 21시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-10-28 13:21:06.873')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'334', N'1', N'1', N'열쇠 60개, 선물 도착!', N'열쇠가 도착했습니다. 게임 내 [선물 목록]에서 수령 받고, 게임을 즐기세요~', N'3', N'11/5, 8시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-10-28 13:22:35.830')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'335', N'1', N'1', N'열~쇠 받아요~!', N'다크블레이즈 [선물목록]에서 무료  열쇠 확인하세요', N'3', N'11/5, 12시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-10-28 13:23:30.627')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'336', N'1', N'1', N'퇴근길 열쇠 선물 도착!	', N'퇴근길이 무료하지 않도록, [선물 목록]에 열쇠 60개 도착~', N'0', N'11/5, 18시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-10-28 13:24:50.387')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'337', N'1', N'1', N'토요일 저녁 열쇠선물입니다.', N'즐거운 토요일 보내고계신가요? 다크블레이즈 하며 더 즐거운 하루 보내세요~', N'3', N'11/5, 18시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-10-28 13:26:01.213')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'338', N'1', N'1', N'열쇠가 부족하다면..?', N'그렇다면 지금 바로 [선물 목록]에서 열쇠 받으세요~', N'3', N'11/5, 21시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-10-28 13:26:46.407')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'339', N'1', N'1', N'열쇠가 도착했습니다~!!', N'매일 아침 다크블레이즈 하며 열쇠 챙기는거 잊지마세요~!', N'3', N'11/6, 8시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-10-28 13:28:24.110')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'340', N'1', N'1', N'오늘도 힘차게!', N'[선물목록]에 도착한 열쇠선물 잊지마세요~ 힘찬 하루 보내시길...!^^', N'3', N'11/6, 12시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-10-28 13:29:29.230')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'341', N'1', N'1', N'열쇠가 왔습니다~~', N'짜잔~! [선물목록]에 열쇠가 도착했습니다~ 신나게 다블 한판 고고씽~!', N'3', N'11/6, 18시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-10-28 13:31:13.870')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'342', N'1', N'1', N'열쇠가! 도착했다!!', N'지나가는 일요일이 아쉬우시죠? 다크블레이즈하며 남은 휴일 즐겁게 보내세요~', N'3', N'11/6, 21시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-10-28 13:33:09.993')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'343', N'1', N'1', N'월요일 아침입니다~열쇠받으세요', N'일주일의 시작을 다크블레이즈와 함께하는거 어떠세요?', N'3', N'11/7, 8시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-10-28 13:34:51.883')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'344', N'1', N'1', N'열쇠받고 다블하고~', N'[선물 목록]에서 열쇠 꼭 챙기시고 다크블레이즈를 즐기세요~', N'3', N'11/7, 12시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-10-28 13:35:42.937')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'345', N'1', N'1', N'퇴근길 선물 도착~ ', N'[선물 목록]에 도착한 열쇠 받으세요~ 오늘도 힘차게 다블 고고~!', N'3', N'11/7, 18시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-10-28 13:37:43.313')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'346', N'1', N'1', N'열쇠 선물입니다~★', N'힘든 하루를 위로해 줄 다크블레이즈! 열쇠받고 다블하세요~', N'3', N'11/7, 21시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-10-28 13:39:14.107')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'347', N'1', N'1', N'점심먹고 열쇠선물 잊지마세요!', N'즐거운 점심시간입니다~ 맛점하시고 열쇠챙기는거 잊지마세요~*^^*', N'3', N'11/2, 12시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-10-31 13:11:08.567')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'348', N'1', N'1', N'굿모닝~오늘도 다블과 함께!', N'열쇠가 [선물목록]에 도착했습니다. 다블과 함께 상쾌한 아침을 보내세요~', N'3', N'11/8, 8시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-11-04 10:21:52.330')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'349', N'1', N'1', N'열쇠 받으셔야죠?', N'즐거운 점심시간~ 맛점하시고 열쇠도 잊지말고 챙기세요~!', N'3', N'11/8, 12시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-11-04 10:23:12.330')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'350', N'1', N'1', N'집가는 길,열쇠받고 게임하자!', N'[선물목록]에서 열쇠 챙기시고, 다크블레이즈하며 스트레스를 날리세요~', N'3', N'11/8, 18시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-11-04 10:26:10.857')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'351', N'1', N'1', N'오늘의 마지막 열쇠선물 도착!', N'띠리링~오늘의 마지막 열쇠 선물 60개가 도착했습니다. 잊지말고 챙겨가세요~', N'3', N'11/8, 21시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-11-04 10:28:11.573')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'352', N'1', N'1', N'상쾌한 아침! 열쇠 60개!', N'아침 선물로 열쇠 60개 도착! [선물 목록]에서 수령 받으세요~', N'3', N'11/9, 8시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-11-04 10:31:46.937')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'353', N'1', N'1', N'열쇠 도착!', N'열쇠가 도착했습니다. 게임 내 [선물 목록]에서 수령 받으세요~', N'3', N'11/9, 12시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-11-04 10:32:28.937')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'354', N'1', N'1', N'열쇠가~ 도착했다!', N'[선물 목록]에 도착한 열쇠 챙기시고~ 게임도 즐기시고~', N'3', N'11/9, 18시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-11-04 10:32:59.817')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'355', N'1', N'1', N'오늘의 마지막 열쇠!', N'잊지 말고 [선물 목록]에서 열쇠 수령 받으시고, 던전으로 고고고~', N'3', N'11/9, 21시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-11-04 10:33:40.890')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'356', N'1', N'1', N'오늘도 힘차게!', N'출근길 선물 도착~ [선물 목록]에 도착한 열쇠 받으세요~', N'2', N'11/10, 8시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-11-04 10:36:31.580')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'357', N'1', N'1', N'열쇠 받고 다크블레이즈 한판!', N'[선물 목록]에서 열쇠 받고, 다크블레이즈를 즐겨보세요~', N'2', N'11/10, 12시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-11-04 10:37:26.710')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'358', N'1', N'1', N'★열쇠 선물입니다★', N'[선물 목록]에 열쇠가 도착했습니다~ 다블 한판 어떠세요?', N'2', N'11/10, 18시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-11-04 10:39:27.847')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'359', N'1', N'1', N'열쇠가~ 도착했다!', N'[선물 목록]에 도착한 열쇠 챙기시고~ 다블도 즐기고~', N'2', N'11/10, 21시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-11-04 10:40:35.933')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'360', N'1', N'1', N'빼빼로데이 열쇠받아가세요~', N'빼빼로와 함께 열쇠도 챙기는거 어떠세요? 다블한판하며 즐거운 하루를 시작하세요~', N'2', N'11/11, 8시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-11-04 10:43:20.747')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'361', N'1', N'1', N'점심먹고 열쇠선물 잊지마세요!', N'선물이 도착했습니다~ [선물목록]안에 있는 열쇠 챙겨 게임한판 하세요~', N'2', N'11/11, 12시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-11-04 10:46:38.867')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'362', N'1', N'1', N'불금을 함께할 열쇠선물~!', N'[선물목록]에서 열쇠 받으시고 다크블레이즈하며 그동안의 스트레스를 날려보세요~ ', N'2', N'11/11, 18시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-11-04 10:48:02.830')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'363', N'1', N'1', N'늦은 밤, 열쇠 도착!', N'[선물 목록]에 도착한 열쇠 챙기시고 다블한판하세요~', N'2', N'11/11, 21시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-11-04 10:49:28.207')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'364', N'1', N'1', N'휴일의 시작을 다블과 함께~', N'즐거운 토요일 아침, [선물목록]에서 열쇠 수령하시고 다크블레이즈를 즐기세요~', N'2', N'11/12, 8시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-11-04 10:54:04.773')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'365', N'1', N'1', N'배고픔을 채워줄 열쇠 선물~', N'벌써 점심이에요~ 맛점하시구 [선물목록]에 도착한 열쇠 선물 확인하세요!', N'2', N'11/12, 12시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-11-04 10:54:41.243')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'366', N'1', N'1', N'선물이 도착했습니다~!', N'다블을 잊고 계신건 아니죠..? [선물목록]에서 열쇠챙기세요~', N'2', N'11/12, 18시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-11-04 10:55:50.460')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'367', N'1', N'1', N'오늘도 너무 수고하신 영웅님께', N'어김없이 찾아온 다블의 열쇠 선물! 게임 내 [선물목록]에서 확인하세요~', N'2', N'11/12, 21시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-11-04 10:56:42.533')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'368', N'1', N'1', N'[선물목록]을 확인해보세요~', N'늘어지는 일요일 오전, 다블 한판하며 하루를 시작해보세요~', N'2', N'11/13, 8시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-11-04 10:58:45.950')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'369', N'1', N'1', N'점심 열쇠 도착!', N'어김없이 찾아온 다블의 열쇠 선물! 게임 내 [선물목록]에서 확인하세요~', N'2', N'11/13, 12시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-11-04 11:00:36.197')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'370', N'1', N'1', N'열쇠받고 다블하고!', N'[선물목록]에서 열쇠 수령하시고 다크블레이즈를 즐겨보세요~', N'2', N'11/13, 18시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-11-04 11:01:36.777')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'371', N'1', N'1', N'열쇠선물입니다~☆', N'지나가는 휴일을 달래줄 열쇠선물입니다~. 다크블레이즈하며 내일을 준비하세요~', N'2', N'11/13, 21시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-11-04 11:03:07.797')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'372', N'1', N'1', N'월요일의 시작은 다블과 함께!', N'게임 내 열쇠선물이 도착했습니다. 다크블레이즈와 함게 힘찬! 하루 보내세요~', N'2', N'11/14, 8시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-11-04 11:05:07.427')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'373', N'1', N'1', N'열쇠가 부족하다면?', N'열심히 다블을 즐기시는 영웅님을 위해 [선물목록]에 열쇠 넣어두었습니다~', N'2', N'11/14, 12시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-11-04 11:07:16.263')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'374', N'1', N'1', N'저녁 먹기 전 다블 한판!', N'[선물목록]의 열쇠 확인하시고 다블을 즐겨보세요~', N'0', N'11/14, 18시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-11-04 11:07:57.343')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'375', N'1', N'1', N'오늘도 수고하신 영웅님께~', N'다블이가 드리는 열쇠 선물이 도착! 수고하셨습니다♥', N'0', N'11/14, 21시 푸시', N'2017-03-04 14:00:00.000', N'test2', N'2016-11-04 11:08:42.230')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'376', N'1', N'1', N'测试', N'测试', N'2', N'测试', N'2017-03-04 14:25:00.000', N'test2', N'2017-03-04 14:22:28.837')
GO
GO
INSERT INTO [dbo].[system_push_service] ([push_id], [game_service_id], [push_type], [title], [message], [push_status], [push_reason], [send_reserv_date], [register], [reg_date]) VALUES (N'377', N'1', N'0', N'测试', N'测试', N'2', N'测试', N'2017-03-04 14:30:00.000', N'test2', N'2017-03-04 14:29:26.953')
GO
GO
SET IDENTITY_INSERT [dbo].[system_push_service] OFF
GO

-- ----------------------------
-- Table structure for user_account
-- ----------------------------
DROP TABLE [dbo].[user_account]
GO
CREATE TABLE [dbo].[user_account] (
[user_id] bigint NOT NULL IDENTITY(1,1) ,
[platform_type] int NOT NULL ,
[platform_user_id] nvarchar(128) NOT NULL ,
[user_account_status] int NOT NULL ,
[reg_date] datetime NOT NULL 
)


GO
DBCC CHECKIDENT(N'[dbo].[user_account]', RESEED, 682)
GO

-- ----------------------------
-- Records of user_account
-- ----------------------------
SET IDENTITY_INSERT [dbo].[user_account] ON
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'317', N'101', N'007acc8fcdcf6322f2648936b4f813e3d65ef74c25bf3b4798fc266112654a4b', N'0', N'2017-02-10 14:54:13.017')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'179', N'101', N'0202b10c33f27639d7fe992645e97641692110b47ec014631a496dfcbae8a536', N'0', N'2017-01-13 10:41:39.290')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'380', N'101', N'0280f7108f313729384b057312f76988a4d3fe160ce675f3675b742e4431edc8', N'0', N'2017-02-20 17:21:23.207')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'301', N'101', N'038778d98f3f8d8b5aea43103cf676794413f28d7487062a11f9010f32aa996d', N'0', N'2017-02-09 15:20:36.757')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'150', N'101', N'04c82356daf35561d47f6659c85031b64a9325cb3cdbc0f1f3ef0f295f771ad9', N'0', N'2017-01-05 15:02:42.010')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'583', N'101', N'051bdef3c9cd10be9c7de96acf9c36b6a50ccee39892f575705f09f36dc4cd12', N'0', N'2017-03-20 11:09:17.790')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'75', N'101', N'052a20ba4680c5fa8d0ba7e46e53f53235e450b59642ab9f145e1f17267aec95', N'0', N'2016-12-27 10:09:56.840')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'453', N'101', N'05ab84d5a798383eaa152c64144419a08bc6aca12b706ca4332e88541af8f72b', N'0', N'2017-03-01 12:16:31.093')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'426', N'101', N'05ae225c0d42cfc5b46c4c6a1a5616f4ee767ed9d5614fecbd0f0b52075a418d', N'0', N'2017-02-24 15:44:43.617')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'527', N'101', N'05eec927fd0caa09ea925a1835525d500d834b49e10b124f9897a9c6a834fdc2', N'0', N'2017-03-14 14:39:34.380')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'470', N'101', N'064153e6c4fe7e67ba9ff88b4c6019b21e425282677741ded90ed52b8044afcd', N'0', N'2017-03-03 18:39:23.983')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'418', N'101', N'0677c7bd2c272a2bfbeb6798b778ac18319719295bee01d505b5bd990b5cff0a', N'0', N'2017-02-23 16:02:27.650')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'576', N'101', N'06aa484db20200b19429ca88eb1482ca28fd73ffe372bc0b61a6516ee043925f', N'0', N'2017-03-18 14:12:26.957')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'437', N'101', N'07409b6ce038ac69f3867fc45ae018e0ea79608f3c7503d3e61bd504482ff9ef', N'0', N'2017-02-27 19:48:07.750')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'628', N'101', N'075c65a79760a7fad3385345885505489fed8f4f99941820417dd825b43b7d3f', N'0', N'2017-03-22 13:37:54.500')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'151', N'101', N'07a97f34fca6dbc19c3f553ee7e132d31a4ee5f67966b9e020382a755636c58f', N'0', N'2017-01-05 16:10:00.950')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'83', N'101', N'0852f71eddc08dd1b7ecb11ad639ddf8da0b9caebf35824a70ce586de472451b', N'0', N'2016-12-27 10:11:48.560')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'483', N'101', N'089e713d8b7ad3e1495dd0b83893d95375b5531b2a4658442fbdb157fc6764a6', N'0', N'2017-03-07 10:03:40.690')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'123', N'101', N'08cb3bcb233048954a0cc3c8622ac58d3b193e3506215490e0a5e524bcb89eab', N'0', N'2016-12-28 18:53:50.320')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'416', N'101', N'08f31fb2f60be07fe9fdf1bcb0341ce2215459709b099671a506b3e4d300e68e', N'0', N'2017-02-23 14:14:37.003')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'636', N'101', N'092696b4e4ab00a3905f8837b9bfe7b34ff969e23777f88afff29d8d4f059a0f', N'0', N'2017-03-22 18:44:57.440')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'630', N'101', N'09a31e936ec3a4d7d11693f035dc9c1af6c293ec8a503104022d278074e4c31e', N'0', N'2017-03-22 15:30:23.457')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'486', N'101', N'09dc170569310b60750d1b251a2ced0e409227f6ef01f4c798470d18fff4d1d1', N'0', N'2017-03-07 14:02:20.617')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'207', N'101', N'0a8f6fe3f081b37ff97b1cce55fde74fecdad62aa3e5cd4c3ebeadb7e930039a', N'0', N'2017-01-17 17:30:24.077')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'284', N'101', N'0adc08340d6fbdc559d3de52ed8618ea9b5933957cbfaa2a036b65d13f76298d', N'0', N'2017-02-08 11:09:11.340')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'587', N'101', N'0b394fb6b60c75b0747ca19ad63405c2ba7339d2920c62d51b74e205f7f1d3c2', N'0', N'2017-03-20 16:18:16.673')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'566', N'101', N'0c267db75d06053984f50e5b98f004e1a01961c9f567d0e819d37eb55e2fc08f', N'0', N'2017-03-17 11:43:30.647')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'134', N'101', N'0cf0b351b9b21337fdf478d3212d6740433354eff19ee9c2926271302479f44a', N'0', N'2016-12-30 14:03:16.353')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'592', N'101', N'0d33383112e1bd007fca26809fdd8b5b8b297cdb0291f3a926cc13d0e7fa35dc', N'0', N'2017-03-20 20:57:12.860')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'66', N'101', N'0d39bdfe02e728a9713c37beeeb226750c106eaf44dfa347319a32952b0d0e10', N'0', N'2016-12-26 13:36:40.223')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'222', N'100', N'0f6551e726c393629fabd39eb5b83607db9ab4feecae85edd604a2da2ae7c9d4', N'0', N'2017-01-18 17:52:59.083')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'51', N'101', N'0f6551e726c393629fabd39eb5b83607db9ab4feecae85edd604a2da2ae7c9d4', N'0', N'2016-12-22 14:06:17.080')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'394', N'102', N'101409657662231339435', N'0', N'2017-02-21 17:18:39.457')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'384', N'103', N'103350836857448', N'0', N'2017-02-20 18:37:52.260')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'368', N'102', N'103789456083086100192', N'0', N'2017-02-18 18:52:12.890')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'412', N'103', N'104186276774855', N'0', N'2017-02-23 11:32:58.937')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'369', N'102', N'104604871339815823938', N'0', N'2017-02-18 19:27:52.680')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'546', N'102', N'105073921907320639287', N'0', N'2017-03-16 07:41:39.677')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'525', N'102', N'105299898084572328693', N'0', N'2017-03-14 11:58:42.930')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'522', N'102', N'110704023211162041991', N'0', N'2017-03-14 10:15:23.520')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'266', N'101', N'110ada72683b50417a821747751f703ec6d4e76f837d28b51a524976953585db', N'0', N'2017-02-06 15:10:01.537')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'472', N'102', N'113915017264953641584', N'0', N'2017-03-04 15:12:06.090')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'559', N'101', N'116eaf9e6788d812cc51afe7e3f6d66e3cd2936254e538d7ed99ee2125781747', N'0', N'2017-03-17 10:09:48.323')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'361', N'103', N'117078048813743', N'0', N'2017-02-17 18:10:01.810')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'364', N'103', N'117098895478325', N'0', N'2017-02-17 21:21:27.163')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'166', N'101', N'11d3310fd2405030abe5ba9e9f8bd985a25a4dacb3ecf8dcb9a945e9fb83c383', N'0', N'2017-01-10 10:22:23.867')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'485', N'101', N'1220bae66e44aec509bba6b82968d9d8e8616368ed6b3f33f0040b984e359860', N'0', N'2017-03-07 14:02:12.353')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'304', N'101', N'126f74ebf43fa1d938608308518330e2b6128bbf1aade33db1065585e00c46d1', N'0', N'2017-02-09 15:26:34.910')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'300', N'101', N'1281b325b1b4209af8070e2303054478a051f9184ff38bb24abf71799e9e7a8e', N'0', N'2017-02-09 12:31:07.890')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'293', N'101', N'1295b665aea39866c3adfd52fe1c9ffdb63f43599cb2381f30e94b2895630b84', N'0', N'2017-02-08 17:14:52.520')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'363', N'103', N'1379115028829777', N'0', N'2017-02-17 20:17:46.683')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'594', N'103', N'141397829719415', N'0', N'2017-03-20 21:16:54.497')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'591', N'103', N'1415683255172954', N'0', N'2017-03-20 20:30:39.737')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'297', N'101', N'14af8b773f7bc7dc1975f0909c192ba44c3a927306e024e74cf0a71646bc6ed1', N'0', N'2017-02-09 10:29:59.347')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'311', N'101', N'158afa502798ca0e8c3b6227f4deeff9d1e3c70c4d196dc899bf054d54186526', N'0', N'2017-02-09 16:30:30.707')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'383', N'101', N'15ac3de6acfefdeaaf9447f3f21625e37ac7a9949b593550cc2d471867000cb7', N'0', N'2017-02-20 18:36:58.560')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'516', N'101', N'165bbf63689e05c524fd04f44d86caeb5728e76f811b028c2adccebf1e7e4b43', N'0', N'2017-03-10 20:01:46.917')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'661', N'103', N'1669692173332880', N'0', N'2017-03-24 18:39:52.930')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'344', N'101', N'172e918bfb53cbeeaed6f0c7ac12e91342a2ae06526d23e4f5de00cf3f98a690', N'0', N'2017-02-15 15:24:00.057')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'445', N'101', N'174d9e2d5034657f29faa320172f0d64a069c27d7095e1b5c7d16a67cbba79f5', N'0', N'2017-03-01 10:38:24.960')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'616', N'101', N'1802a9acf5225ed5bdaee1fbd7cf14941926de4502932a8c1b26b92e50f2e31b', N'0', N'2017-03-21 21:33:54.020')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'360', N'103', N'185796248573090', N'0', N'2017-02-17 12:02:57.740')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'167', N'101', N'1924c43cbbdd4a79b7fc539c0b2a2d8010ccff809fe8b48f9af520bb46e094f1', N'0', N'2017-01-10 11:54:38.540')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'518', N'101', N'19905b0bb6b6c19fbd6621fdd086236e964bd221dff274c1352e7aaccec69df4', N'0', N'2017-03-14 09:52:14.277')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'656', N'101', N'19a1dd12330c1602d95736d838274fb4e7ee0d511aa1a021e7fa34a814019720', N'0', N'2017-03-24 15:09:03.417')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'59', N'101', N'19b8c1ccd1a9d32a794220af86c4f921a0626c89e57dfca9fdadad1a61f24c21', N'0', N'2016-12-23 17:22:58.187')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'581', N'101', N'1b7e36a20a3228094de48901cf36d8f16451b0364b55e4f6a65bdfeb28aeb081', N'0', N'2017-03-20 11:09:05.933')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'97', N'101', N'1d4a44400b9cdec61e7d3eb6263635e0309b9856d5dd2224187feee3ecd1280a', N'0', N'2016-12-27 10:58:52.400')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'321', N'101', N'1de6b3744049bc393c0d8601a563302b942e3a356b7ce59d690d077b083c827b', N'0', N'2017-02-10 17:01:09.070')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'607', N'101', N'1e1644f49d4629c43805d6918a69d36fc64072d129a9d295812525b7445dd339', N'0', N'2017-03-21 16:17:09.457')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'621', N'101', N'1edb7d22bd5b904c1e8c9cff7f10c558859d8f6a8053da776e956a990a2003a8', N'0', N'2017-03-22 10:22:04.673')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'318', N'101', N'1ef928663b0d0b42d353b601c9e09a48448f33c93e1a1b8c5cc7423009b5e115', N'0', N'2017-02-10 15:48:32.770')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'323', N'101', N'1f15c26804007426412ecaa0406fad8007205025a66f5bbd02d1cdcc8e357413', N'0', N'2017-02-11 11:04:13.073')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'282', N'101', N'1f7b051c77464907b715342c581f3b40652f3fa9b8989622fd4f7b79c9c91e7f', N'0', N'2017-02-08 09:56:36.993')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'427', N'101', N'1f83382ddfc5f9dc276135c5c7c59e41f80be033bae6848c511fc1bc86155e82', N'0', N'2017-02-24 16:42:31.027')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'653', N'101', N'20b283aac9c05ebf3f32826fe9863e69592b10b8a1401461e6aa9686c7939e38', N'0', N'2017-03-24 00:26:31.417')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'514', N'101', N'212b381d86fe6c7c0b9f20e2fbf039574dc2d8f9f7f947d5a5efa0b64e764bcf', N'0', N'2017-03-10 13:01:40.330')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'488', N'101', N'2148b579f1404c3d1661446ae7afcae32e9d2c453d965cdc64b6d0de787fd290', N'0', N'2017-03-07 14:50:35.623')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'159', N'101', N'21c5dd37865b7101cf3558e709b37e5335bc4c11fec0d15c86cb151836d8eba8', N'0', N'2017-01-06 17:05:47.290')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'102', N'101', N'21ebb91f8caf90ade9612251aef3be16ec44e9c5b6e418c975dcc467e98e6667', N'0', N'2016-12-27 14:32:21.367')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'210', N'101', N'2224cdf9a34b43ed6c24fc0ab220f0ab9c7a2902458f35e09a6f8c3cf7e5436c', N'0', N'2017-01-17 17:43:55.047')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'523', N'101', N'2283c0ccd22f1eefd68fc1e70ae808365cbc976d7208eb317215a1f81d95e80f', N'0', N'2017-03-14 10:22:21.257')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'415', N'101', N'22bde1bc12369d56a21a4545b470ac83eeb069397d75b8011ffd941630593c3c', N'0', N'2017-02-23 13:55:29.023')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'376', N'101', N'22e6cf83bd3e3160d90d7a06e03f6d8526cee90890c69d2441083c03b8985600', N'0', N'2017-02-20 12:07:00.653')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'157', N'101', N'23acb729cf6bf62d45fd4bf0f5183041f3b3882e3dbc7a99c1d0a6616d560fc4', N'0', N'2017-01-06 16:05:36.640')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'221', N'100', N'2496a3b4a1c5ed114697e056c780913df285fd4952a90f12a66dc5979d6efb89', N'0', N'2017-01-18 17:52:39.253')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'111', N'101', N'2496a3b4a1c5ed114697e056c780913df285fd4952a90f12a66dc5979d6efb89', N'0', N'2016-12-28 10:49:04.923')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'499', N'101', N'24bb5f2e18d921a26170dc5e394370cb21322b68ac27d45fc2680666d56e774d', N'0', N'2017-03-08 16:48:59.937')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'143', N'101', N'24cdb9442da0c48785f72bd6b6ab92f334556ed6b5f52a72021583b901adf78f', N'0', N'2017-01-03 13:35:25.077')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'245', N'100', N'258926e15cf34b973ab93e271be2fbef4989ada848069684dddae7b46c7af939', N'0', N'2017-01-23 15:52:20.290')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'60', N'101', N'258926e15cf34b973ab93e271be2fbef4989ada848069684dddae7b46c7af939', N'0', N'2016-12-23 17:57:09.033')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'668', N'101', N'259cf3d5e1ff6be3d79bd9c914f673ded750e3319f47f50669aa8c12d1bb7b02', N'0', N'2017-03-27 13:29:06.387')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'646', N'101', N'259d04a8e557fe498e5d62769281b670870a6c16ed49537ba5024d282387c6f5', N'0', N'2017-03-23 15:32:51.843')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'478', N'101', N'25ad5ab2bac9706c4226ab9d5cd24d4c1ed81d74f8d1dd85d6125d6aa7aeed86', N'0', N'2017-03-06 16:55:34.033')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'177', N'101', N'25d649e876396bb94b544515aeb07074f1fd87caf584f0e49ca68d68e6c13647', N'0', N'2017-01-12 19:35:45.197')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'577', N'101', N'25d7804a5a0d795e84a1198fbd27b10c605976e01a979024241de620c0dd9c98', N'0', N'2017-03-18 14:12:28.147')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'563', N'101', N'26da300849b097a69ca4c7df35e197c70885a9496018fc7e93b64ab89c2341d8', N'0', N'2017-03-17 10:45:26.903')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'554', N'101', N'26df9bfbdd4982eb833f08714609d597a8fbdc31f37a22bc9ad5f86b5b878187', N'0', N'2017-03-16 17:52:48.197')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'410', N'101', N'279ef717eb750c6b9f373c7316f4a88e5425f405d47b5f95369deece70ad7515', N'0', N'2017-02-23 11:15:16.550')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'597', N'101', N'28ed5b5bcbc4d810facb6df6ce5f3e52475e6eb2fd80a2af45693719239775d6', N'0', N'2017-03-21 14:08:28.140')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'584', N'101', N'293eb238c1bad6b6a89455309c67cd6ed52063390f8677fba6dd35f3b7834fe3', N'0', N'2017-03-20 11:26:19.540')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'463', N'101', N'294279b7b3bc02217e3c2d6d73a810a1d9e4b22863095e4ee83ebc154c78182b', N'0', N'2017-03-03 15:19:45.400')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'356', N'101', N'298fb900e1d248c952312f0999c94aa8edc8f79080c8125dc38ccb54030797c6', N'0', N'2017-02-16 11:51:11.113')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'455', N'101', N'2a6481f38ec9aa95a9900cfc6f864d2fb837a9bafe3d860ba662a6e805a76f72', N'0', N'2017-03-01 14:19:53.027')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'165', N'101', N'2a7a029056a22bdb3f5fa8e8b7957f18bf8f952286c6d8ec8a010f600070e98b', N'0', N'2017-01-09 18:23:11.007')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'652', N'101', N'2aa02e64f51b9ffaae949a663fbfa2199967ecd0865f1a9bffaf2570927be1fb', N'0', N'2017-03-23 21:37:32.853')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'190', N'101', N'2b748d67d35c5869dd8395aeb9b5d9cbe6b328b6d7e0061a01901a92c4896637', N'0', N'2017-01-17 11:33:45.427')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'682', N'101', N'2b79b0bc0f129e35645391b547f3d50252f335eaf8742b2b229569733af0f4de', N'0', N'2017-03-28 21:32:56.827')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'335', N'101', N'2b898e946313cf74923c246f322ce2f2715a5ee3d4f03ce80b94660fdd0f12a8', N'0', N'2017-02-14 10:34:03.090')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'400', N'101', N'2d4c6a2a63b080c17aeb2a81e3858a0092635ba63883415be79af00e5ca0de04', N'0', N'2017-02-22 15:00:27.160')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'547', N'101', N'2d4cb9690db6fdd46aede8345e97d41e9af4725d78cac7ff6c5db5b93f7523e9', N'0', N'2017-03-16 10:00:29.513')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'191', N'101', N'2e16b09cda3b9877441202929423cc898a56d860d2752d6f8c29bc10110fa343', N'0', N'2017-01-17 11:52:29.830')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'520', N'101', N'2e72d85339b1b6dda5ed29039181eebb3fe8da45b9cbc82c912b5f362e5f723d', N'0', N'2017-03-14 10:08:33.460')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'114', N'101', N'2efe5170caf0244c66ffc46bae1fe5ae301b7f088d4e76756af6f84fc377c9b1', N'0', N'2016-12-28 10:53:37.650')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'387', N'101', N'2fce38164d78cebbae0db089887126df08ade00ae624eea7c5adda708da11d32', N'0', N'2017-02-20 19:11:17.917')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'198', N'101', N'3075a4efe76ae45e5612d626faed977671e4caa4e90c287227679020c31ec157', N'0', N'2017-01-17 15:14:48.783')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'605', N'101', N'30f8108f0f5e4d09f2bc1423d172091a143c45f6a6f9e676961cd3340cfa0a6e', N'0', N'2017-03-21 15:51:10.320')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'246', N'101', N'31414ae3f60a69680eca020204bd377cd2b1b2f22632854049e60d0a7e511176', N'0', N'2017-01-23 15:54:49.997')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'68', N'101', N'32b2456e904e5e58cc64ed94ada4515a053b599b73bf14d9912e27a31b1ffb4b', N'0', N'2016-12-26 14:25:07.347')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'41', N'101', N'33587184c3821a7463c52b29d9d076fe651a304753f02a8ad575cdb0a1f29168', N'0', N'2016-12-21 20:51:39.290')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'131', N'101', N'338dda501c03080989f9fb44dd4c485f35b6f391cdb22cb8fb18754a0c2fdfab', N'0', N'2016-12-30 11:59:31.530')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'556', N'101', N'33fd9d6eaa0a35cd6a9f3ffe3b87d9c6760cff411a62cc7d8cc3f2505bd23fc6', N'0', N'2017-03-17 08:39:50.590')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'443', N'101', N'342eb42fa6ce7014379b31c78b9e0df71cf95d09ae12adeac0eb1775a013d888', N'0', N'2017-02-28 16:43:35.273')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'419', N'101', N'3560a6d26809d3a3a3fa050de6c6975d7963440b0d29855bac093e50366253ba', N'0', N'2017-02-23 16:08:54.727')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'211', N'101', N'358b1b106045b10312f64e30078594fcfcec9600c8ee7e5a8ae023d19a43c364', N'0', N'2017-01-17 18:18:53.303')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'152', N'101', N'3593a31ee8c8cbf9282e02620cb14280a48833cb35cc4d1da53efce64db94c83', N'0', N'2017-01-05 16:14:33.330')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'557', N'101', N'35e2c3b30f8150d3086f50aa1496f5fd25f328e9de90a63b37bf6457891b5465', N'0', N'2017-03-17 09:51:17.680')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'663', N'101', N'36df9fcde3256324cd17ec6207da53c2de01a3282b606ba58b630089bb7da6c8', N'0', N'2017-03-24 20:10:03.550')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'252', N'101', N'36edda42d12195c5e3a85c3cb7d07f2124af7f8d5468c77383ca7f6978da6687', N'0', N'2017-02-01 19:37:18.580')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'637', N'101', N'36f2d0e8a922bf587dcf9b9fa6a5b379a970f6ff65c4f5ac406e5c767b612246', N'0', N'2017-03-22 19:03:50.747')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'609', N'101', N'37fb82c35cbf540d392cf1f1280403b42459ada23753160c5f43c58323ada66e', N'0', N'2017-03-21 16:52:26.837')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'580', N'101', N'3922f02e59460032f3254e4b0fddea3dd11ba2415da03bf22b5543f78f46c3e4', N'0', N'2017-03-18 17:27:04.940')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'623', N'101', N'39552489f3373e236cb603b2084f481a8eda28ca7f8c971a29efe2aa2dc08620', N'0', N'2017-03-22 12:03:06.347')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'274', N'101', N'39940e6d0603d9da151507cc044b55a6b3395c98ae6987ca70a651753fa880e9', N'0', N'2017-02-06 17:12:17.020')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'78', N'101', N'3ae953a43836397b1b337965071be397831330d124caad5a55975c306ff8de5e', N'0', N'2016-12-27 10:10:21.927')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'552', N'101', N'3b6c120de05e0d86edb7620c5d280f24bdef108cdf41982f2a6ee0dec9ff3c90', N'0', N'2017-03-16 17:40:08.893')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'296', N'101', N'3c0117971f1d039e1c8931270ea6b3d2a539933ad64a84f3ac46f988d07ff8cc', N'0', N'2017-02-09 10:18:08.240')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'365', N'101', N'3cdbb8961c937d97f0cecdbca4e5b1bf5b63328358ec6a145b66ed34c134982a', N'0', N'2017-02-18 15:42:15.220')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'47', N'100', N'3dae798e65596d6a16c3e493d7447fee5edb1ec1d6052a9974ac0f712ef83cfc', N'0', N'2016-12-22 12:08:25.277')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'38', N'101', N'3dae798e65596d6a16c3e493d7447fee5edb1ec1d6052a9974ac0f712ef83cfc', N'0', N'2016-12-21 19:03:55.400')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'638', N'101', N'3dc0d7cf447857bbbef1a1d349c30f48c33ba80ff5ad5225d6dfd9dd29fdc406', N'0', N'2017-03-22 20:00:53.907')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'261', N'101', N'3ddd7126a525acd4cdbee559abe9f003c80199dc106f2ff979d3cdeb28e47096', N'0', N'2017-02-06 11:02:14.727')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'399', N'101', N'3e6c0becac326c82dabd8fd929cbbbdb527d99918ce635489c2b678f922fb7f5', N'0', N'2017-02-22 12:47:20.643')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'446', N'101', N'3ed21d13edce99485cdee35aae2f12e130d3ab25f09601bc6bb16d7646364d75', N'0', N'2017-03-01 11:08:09.557')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'270', N'101', N'3f347bf9c84321a4f703b7935c695f07b3395c98ae6987ca70a651753fa880e9', N'0', N'2017-02-06 16:06:04.653')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'330', N'101', N'3f91106662464c755b80084fec5999787a9fa5f18d75ad3b8f530fd440497891', N'0', N'2017-02-13 15:32:23.597')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'534', N'101', N'3fb9149c8f8eb247ce5e163ea821a09db7d5a9c331baef6d099e6dcded3e21ea', N'0', N'2017-03-15 11:17:59.603')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'155', N'101', N'3ff67eaca2868a6afe8694c09e9b869dfb42c244b30ada82476e2da5de44c314', N'0', N'2017-01-06 12:10:37.257')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'116', N'101', N'4027f7c61c20f96fa2fa40622853050b7af00b99e9ac8a91cb68ca8d7aa4b6c8', N'0', N'2016-12-28 14:50:57.617')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'503', N'101', N'4059647fc024dddb756f74b44c30c887012edac72d20770fbc071f9979b77703', N'0', N'2017-03-09 15:21:47.600')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'392', N'101', N'4079fc15865cabca80cb870fdcc700d242a2ae06526d23e4f5de00cf3f98a690', N'0', N'2017-02-21 17:00:25.477')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'212', N'101', N'40889c0e6b79bb092df0013f2891729285eccbcd5a335a700eca7539e19ad2ad', N'0', N'2017-01-17 20:02:41.150')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'417', N'101', N'4093b1d48187e18cd2c6097775a40afcda05a0934b4f2643df890d8e2556fe5a', N'0', N'2017-02-23 14:24:31.593')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'442', N'101', N'40b2171322da7cc2d330a8f8f25d52151cf95d09ae12adeac0eb1775a013d888', N'0', N'2017-02-28 16:38:03.817')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'629', N'101', N'40cc684dfcd14a669bbc2c5e3f79a798a41fdd560091b5829fde9db93b486fb6', N'0', N'2017-03-22 14:01:48.020')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'168', N'101', N'40eae2140da6cecda79121f5c03991fac5dbe265653c1de66362b4ed8689dcae', N'0', N'2017-01-10 13:23:41.067')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'393', N'101', N'410ebbe5e91aa75a9135dacb1dcf231fe48cb870455b4e68cce0d77dc5af3820', N'0', N'2017-02-21 17:02:07.160')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'674', N'101', N'4140c24ffc6afb3eaf09b4e8811581b0c3568791a7c23ab7e45f57dfbd083b88', N'0', N'2017-03-28 13:35:22.807')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'352', N'101', N'414cd49896d15903f8aef0139411cf941cf95d09ae12adeac0eb1775a013d888', N'0', N'2017-02-15 17:50:12.093')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'632', N'101', N'4278912de134c554c1ba2fb801bcebf3f47d16195196ead9100265eefcf66262', N'0', N'2017-03-22 16:44:06.747')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'160', N'101', N'42912c75c683e315c5d68f2bf09915febb5443210ef72bf5092049d4e344f7c0', N'0', N'2017-01-07 12:30:02.243')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'170', N'101', N'44438141f495f136aef2b5350efccec186a9f45988bd3bc57fc05657c9171715', N'0', N'2017-01-10 16:36:25.973')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'492', N'101', N'444f97ed0c92a43c77f04b5ecb493292641dbd55bad102331e0812fe4d2f6656', N'0', N'2017-03-07 17:07:03.590')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'477', N'101', N'446ebc9cfcc248be13767190384d08499b04153aba6a42f4ff11de21b3774cda', N'0', N'2017-03-06 15:43:55.210')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'618', N'101', N'45191e6a2e4e89a3b54e7c7a27e2f9ed0df6c0ecc2856cceadbd1c2cd8986fcb', N'0', N'2017-03-22 09:44:33.123')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'558', N'101', N'45ca5a23dcac374eb5f39b313458c9f8f0c2fe37fe1049f8fc4067367b933bd2', N'0', N'2017-03-17 10:01:02.950')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'204', N'101', N'463f37ec77ef3b8b44d0e383ba8f6860d263bbc4d0f837ab84aadc327e4ed688', N'0', N'2017-01-17 16:57:22.907')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'539', N'101', N'465b2e032643d4f112ba68939189cecfa24ae1126d8e54ce8806443018ee8dd1', N'0', N'2017-03-15 13:00:34.803')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'164', N'101', N'466009cc6361d48ec1280a78fa9023dfe4bd20d9c90e82adf4d27fe4818b0c82', N'0', N'2017-01-09 18:18:46.497')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'467', N'101', N'478dc4ec90e16e59ae6d308b68d9d3b634290c0cfc0de69c1d0147d851a0ccbf', N'0', N'2017-03-03 18:04:53.043')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'545', N'101', N'47a840b91547ee0b6d91e1a92f7841d2df9b846141298b23e6e0265b734f618a', N'0', N'2017-03-15 16:21:07.027')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'667', N'101', N'485058f1a56aa5b4ea24f9fef5b5c8e93cb89afffe3432d909a093fec35560a9', N'0', N'2017-03-27 12:26:27.570')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'146', N'101', N'48886eec73e445cc9b3eaf13124ec6c8746b697de4c3a2147526e75863d9664e', N'0', N'2017-01-04 15:45:48.890')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'413', N'101', N'495096bb50d7cb6af87c8b3f0ae8f305f7992696055039000ab4fe5fed03c032', N'0', N'2017-02-23 12:06:45.690')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'680', N'101', N'49756c363d045819b1c022384f3b0813bde8bff43b60af5421c18c4944d8ccec', N'0', N'2017-03-28 19:07:56.637')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'303', N'101', N'4986cedabb61b44181d63e4ea2484d4b2ad6dd22e3d46a50e91db195b4d8ff75', N'0', N'2017-02-09 15:23:45.570')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'450', N'101', N'4aeba78f197202cbc6feb6a307959e91576625bab0705e646cb514f849bae0ba', N'0', N'2017-03-01 11:14:08.067')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'425', N'101', N'4be2b84bc67a2ce61f8cfdc9f106b3ca22fe3dc70d71b1d57a6fd159fe005464', N'0', N'2017-02-23 18:40:26.590')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'465', N'101', N'4ce3c08fe6e0f57f0e4a139a051a9268aa28dd42120a7d102256b810a6b484a9', N'0', N'2017-03-03 17:08:33.500')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'362', N'101', N'4d8ec4f0cb5030e8f4f1b31b2e5a38751cf95d09ae12adeac0eb1775a013d888', N'0', N'2017-02-17 18:12:47.910')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'572', N'101', N'4de3f10aafd8db533766eefc80b9f009ac28f9a7c6e6b57548384e53141f80d5', N'0', N'2017-03-17 18:52:49.600')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'550', N'101', N'4deaed703aa0d4a8544b3527453176f06871cbbd84ea48ac516a40d7ba03b0d8', N'0', N'2017-03-16 13:33:17.923')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'254', N'101', N'4fb4641416ca45abac782cef7f8e3ca5364d1bdb42074233a54c213887ec8075', N'0', N'2017-02-02 16:35:05.927')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'471', N'101', N'508831b9176f7da1cedfaa237930b3d44275dac0a56966806cb6cf62c09eb3e0', N'0', N'2017-03-04 12:26:59.753')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'148', N'101', N'510ea5b7b4ed93c374b7ba4c1da1b7449bf4d5ec991850a2fa2f75a7d12c4b2c', N'0', N'2017-01-04 23:41:56.580')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'541', N'101', N'51b1e19a7d921c954c3092033caa08473d2cb1717d71c8ea591116165692ef12', N'0', N'2017-03-15 14:31:12.510')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'643', N'101', N'52f35a4f818ec52a21cd8b099354a9b21f4b31a4d519d67b48e43abb42c7056f', N'0', N'2017-03-23 12:44:43.377')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'377', N'101', N'53452d8535ec43f494377d2d25321cd889ad70da80c809ab0b1f6fbec89223ea', N'0', N'2017-02-20 12:09:40.427')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'249', N'101', N'541c27c31cdbeba824097eb0028db07882720b4e6534c257b801b7c7b8ae2f24', N'0', N'2017-01-23 21:19:08.630')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'182', N'101', N'55840658e347a1207d74b872653f03708e1e66fcbea2b8ef0e9c33b7060b15ce', N'0', N'2017-01-14 18:56:04.797')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'434', N'101', N'55c485831310b8789f11a81725418c4a72821e26e68f99895d2bb646b63114db', N'0', N'2017-02-26 15:46:05.163')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'504', N'101', N'5659425d31fd60660dc6ef278bee3b047797dcd179bf3cea8181c3024d256dbe', N'0', N'2017-03-09 16:25:10.357')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'192', N'101', N'56f54a90bae18eb2597f95ef41baa3978a56d860d2752d6f8c29bc10110fa343', N'0', N'2017-01-17 11:56:51.313')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'507', N'101', N'57da39b903f7a37058a7377906bafd4838ea622f4781e50b57f24bd861c91cca', N'0', N'2017-03-10 11:31:03.893')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'44', N'101', N'59af00bf2d8f4d47bcec00e4dee7942fc014b0b98341c292bf49a8c31ae6429d', N'0', N'2016-12-22 10:59:59.030')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'509', N'101', N'5a2df86129a1f3ca9551fd55e41632be27957e969c2144aca8f10c82ab9f38bd', N'0', N'2017-03-10 11:50:48.957')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'219', N'101', N'5ab269358b47cbcb7067e012b10dafa39c9d99974da6c939c5dd57903d9fb109', N'0', N'2017-01-18 14:49:40.673')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'612', N'101', N'5ab5db3f73d4cef42ab1d6c974bcf750e1ff58c8b149b9641f7f25bb5a2adb53', N'0', N'2017-03-21 17:46:54.827')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'440', N'101', N'5ba308d018599c7f5f0e194974481047b55721033dad0b011bd2252faeadd03a', N'0', N'2017-02-28 10:23:46.750')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'428', N'101', N'5bac967660e30184ae3738e757329426c89269329c981b0505f6312864d0be54', N'0', N'2017-02-24 18:06:49.777')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'651', N'101', N'5bcc8871853d5ba80723f2b37203086adebdd4cdf36fdb570c2d9b1d8249a5c7', N'0', N'2017-03-23 19:43:25.380')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'619', N'101', N'5c254096b2fc2d217f9af367f9b59fc909b82c5639221f1000224a32458a8f95', N'0', N'2017-03-22 10:03:16.130')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'64', N'101', N'5c28dbbc44b7b0b4e3395ed12d677f80fbd0e1138030f81d539c8cef6dbc3884', N'0', N'2016-12-25 23:40:40.343')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'567', N'101', N'5c761d1de3939ac329cf5c2808c3c2bf236353e14af6367d7c5ed2bd7ca6dd23', N'0', N'2017-03-17 14:40:02.040')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'289', N'101', N'5c847170ff76601ef8ee995f1b2b5f1161a66fdfd3a83a80a75e53fb0ce16301', N'0', N'2017-02-08 15:03:33.153')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'660', N'101', N'5d4aad5d335bd2767d39227d7b4e9f6c0c5d16e6bb5c2694eda13bee1b242e1b', N'0', N'2017-03-24 17:56:10.250')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'532', N'101', N'5e3d60b35ea4ad7fa0a409da655b01d3b183f2acebcb325432ade99b15c8a278', N'0', N'2017-03-15 10:29:54.580')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'156', N'101', N'5ea0610c27101b87d7352df8a849038300e08d08036c60cdaffa6e6f4816b32f', N'0', N'2017-01-06 14:04:35.370')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'562', N'101', N'5ef2b3e1ea008ac3a1545223a070df21b9ad48c31c460f074222c738c0b522fb', N'0', N'2017-03-17 10:24:16.663')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'63', N'101', N'5f19479aab91629371876c5a51fdf8e15472c078f9ca2d8189315213ed6071a8', N'0', N'2016-12-24 10:15:19.223')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'624', N'101', N'5f8dd7d264f8e291ee8d4ffb9a3812308518629db7c7e7d8d0dfa580f08a2718', N'0', N'2017-03-22 12:03:53.723')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'533', N'101', N'5fe6b4697a54e1c760d34655760a1509ad015e82547f901a993f63e87f3e0c92', N'0', N'2017-03-15 11:15:54.350')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'480', N'101', N'60351c7d3893d08379f79c0b1743f6a59d174dbbb49f2570d60db6b8a9f4d712', N'0', N'2017-03-06 17:31:34.483')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'336', N'101', N'6039cd64a07b48a90f182e4e2c7c6cdd2f5aad538f4101604343f586523149dc', N'0', N'2017-02-14 10:58:06.043')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'278', N'101', N'60728990bc9879866f7813feed90945b704ba5a3a998d6023be0359d4e9d138d', N'0', N'2017-02-07 17:22:56.213')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'141', N'100', N'609ab334c03e2d881023dc9e7f09ca88cb0a2601f97ab204de4ffbc012ca7efe', N'0', N'2016-12-31 16:51:29.057')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'139', N'101', N'609ab334c03e2d881023dc9e7f09ca88cb0a2601f97ab204de4ffbc012ca7efe', N'0', N'2016-12-30 19:03:38.453')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'489', N'101', N'60a3613afe8a14273f37087ff35582916faada1a36d10ce86d227c00420aa1ab', N'0', N'2017-03-07 15:43:04.057')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'401', N'101', N'60fd76272a7ed4e8ea142efe78b67cbf4a5607be2e2e8a92305abcd2b7538d4f', N'0', N'2017-02-22 15:08:10.493')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'180', N'101', N'612b98fdce94828294086229184a308feb493ceb470bc55d27f42b05fb083a4b', N'0', N'2017-01-13 10:57:33.120')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'145', N'101', N'612fd4c81b4efa11bc6e1e700538ff987e6b9a22429ac1f23cd0426ed1b13283', N'0', N'2017-01-04 12:21:44.300')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'374', N'101', N'6168f6b4e993f056c07e6b6c93ef2bd283b974877d3ee7fab7115b82eab557c0', N'0', N'2017-02-18 21:49:23.347')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'255', N'101', N'61af09119684c0e7d1d077f3bbea2850098b5be8755ff5b70d6e2c5427d141c4', N'0', N'2017-02-03 12:33:21.493')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'476', N'101', N'61d471388bc532b74d6ee6c2dd706d6ce2adfff7f8516468c21c4d78d56bc990', N'0', N'2017-03-06 14:51:02.800')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'224', N'101', N'626a27c61d0b65b9b24f933f0d0f8443e981b32ed6689eb34087f1b5ab7398b6', N'0', N'2017-01-19 10:01:09.177')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'372', N'101', N'62f07cd704e7a63a94b50b2af0f1f55515d4c6b3fc87ee810e43c141d18c3c62', N'0', N'2017-02-18 20:49:24.643')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'447', N'101', N'62f41294a0c121aa6b45625bf9c74c8c9af30757ee5deb042a86c73b36dd64d7', N'0', N'2017-03-01 11:08:45.783')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'281', N'101', N'63e6e6cc5b89b3f78aee4a655184ccafe5c8f68f7b56271939bda8ab13b7755c', N'0', N'2017-02-08 09:52:11.460')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'655', N'101', N'644a95678b3834b3e4afe1c4d743bb03cd7d264e1b080bf34af85b48b887356b', N'0', N'2017-03-24 12:05:07.097')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'444', N'101', N'6455a245c5911e237c26bb8f564cd46d26e297b80e8898beba36ec304e91a9ba', N'0', N'2017-03-01 00:55:12.917')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'337', N'101', N'654594fff873871a56d7e4dd39cb29e1d12a551bef9f83545a0776619e19a898', N'0', N'2017-02-14 11:57:54.107')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'176', N'101', N'655ab3b12876a85a8864f5253ae3377525adaf13bb7e7f40fd5f478944030b75', N'0', N'2017-01-12 16:20:05.183')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'265', N'101', N'6662ac011954cf60ca63b6aff817a82770da52a066ad2aa64d2c2c8d5f878d42', N'0', N'2017-02-06 15:08:40.167')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'227', N'101', N'6665b1f1a5efd331ee79dcd16f6e2af4fac22af6dff8596388dc93c2297dadc9', N'0', N'2017-01-19 15:35:59.107')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'315', N'101', N'66e0a3ef21e3e97a0d37edf488f560bc338408b2ac772fb98cf713d8e4a85715', N'0', N'2017-02-09 22:54:23.740')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'213', N'101', N'67e0673b5c4b121901bddbb67e79de480e68ca148d260e50ed361071344c21fd', N'0', N'2017-01-18 10:17:20.017')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'681', N'101', N'692b6adfb6b31c692a3236e448401494429f8db4004cd4d80c00395c1a553fa3', N'0', N'2017-03-28 19:42:27.180')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'220', N'100', N'69ff204ad5270ebc3fc80773636c12c955acd4988169bf4815e851c4b47ca9e8', N'0', N'2017-01-18 17:51:54.430')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'130', N'101', N'69ff204ad5270ebc3fc80773636c12c955acd4988169bf4815e851c4b47ca9e8', N'0', N'2016-12-30 11:10:15.460')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'333', N'101', N'6b1351e21a5c89663ec1fa1508887398a1730e2fe4d433700b90a9e34fe8fb7e', N'0', N'2017-02-13 18:09:14.950')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'633', N'101', N'6b3930e1dd5464e11488d57bf7e6f87b10ade3690706cd0ce02d357da735635c', N'0', N'2017-03-22 17:40:09.353')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'468', N'101', N'6cad95a57236e5bad8548ea9fa4486bcce642a1ca2707346f29cb96d4ffe9685', N'0', N'2017-03-03 18:17:27.690')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'664', N'101', N'6ce1001910a4f26849daefcf92b59925f3fabd4c43e6b193f772f53bbde2b6da', N'0', N'2017-03-24 23:50:47.363')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'639', N'101', N'6d4ea5c7c7327606a33280aaf1079cc463611700e0c1f5172fcd73a043f0bbc8', N'0', N'2017-03-22 21:55:27.467')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'77', N'101', N'6e232f30d1eda71e02710ebaa8d8409517c3e06b79b53ce438e738923fc2c6de', N'0', N'2016-12-27 10:10:20.167')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'540', N'101', N'6e6b163dd4ec2f696767e92e49796e1e5a3e361191babc0b8aa8a01cf1ea703e', N'0', N'2017-03-15 13:52:20.110')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'404', N'101', N'6f278a34e2dd27d69af55a6f125d733f7f8f2d82e2a228d11c47753bbec01bdd', N'0', N'2017-02-23 10:02:11.313')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'536', N'101', N'7015000e7675d1bdf1a59cb7b1b1f19bc8a4f66215cae045a22f94e99c047c98', N'0', N'2017-03-15 11:53:58.450')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'614', N'101', N'70237cc0d7f085d017fda3a896575267e2b8c8580dc5a72aa49882781552890a', N'0', N'2017-03-21 18:33:49.850')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'67', N'101', N'7025405f529cde29a6145aff1ed0d37da73681973563aed0fa9ce27b857f439a', N'0', N'2016-12-26 14:24:40.177')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'648', N'101', N'70887e6f77b34251db5e29775ece7858b01f9becbd3ba7e1c593dbc8889c2744', N'0', N'2017-03-23 16:54:55.850')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'110', N'101', N'715b5607e3b28c663d74e68bc3b4c9d690026510865f4079d7a7df6d4b004c34', N'0', N'2016-12-28 10:25:16.960')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'86', N'101', N'71a7352b60587b8f8f1d3335c5248e709740110e0573ad0d470c217f4602181d', N'0', N'2016-12-27 10:12:20.073')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'669', N'101', N'71c02383d73c1dcebfcd8300678d5e38b2e60c84d936c32d6cd84e507e9b1b3c', N'0', N'2017-03-27 14:23:16.430')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'174', N'101', N'71d8742f9dac35151fa963c33b459960b6f355428dba15c6cf510ba00fe5d380', N'0', N'2017-01-12 09:21:38.553')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'206', N'101', N'71e6acc81f6528a8682e56a03c0fd98a2c79d12ed1e44bf9b819dca7f574b68b', N'0', N'2017-01-17 17:25:18.197')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'253', N'101', N'72ef5147d2de3a285f3a7afd4cfbb964ab56a78b71cac9ef821a25f2f1d7bce8', N'0', N'2017-02-01 21:19:58.043')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'421', N'101', N'72f873e872655a7737dbc0c3a0d32d1feffc9a66bcb983a9d470823d7abaaf54', N'0', N'2017-02-23 16:37:29.350')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'433', N'101', N'72f91850cffd139e88c6a37e384d531083d274bfd61ffeb9bd45249eaafa539d', N'0', N'2017-02-26 15:30:54.143')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'197', N'101', N'73b85167ca5a6bc9c80e43e97559d476719d2e8366f1a80ab0e725b21dcc5630', N'0', N'2017-01-17 14:44:59.913')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'161', N'101', N'7421a6f4614be74c6111cb29442428b678594e965d834cd8c4375b50c5201a6e', N'0', N'2017-01-09 10:24:58.833')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'340', N'101', N'7452fc0919d5396d491ab42c835755e77b3078071b2f836d9291d142711b3d13', N'0', N'2017-02-14 16:12:50.040')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'287', N'101', N'74f339e010ebebc5e648ada68e62af50c8cdb5393fdbb3a5505f26d414841c87', N'0', N'2017-02-08 11:59:56.633')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'120', N'101', N'759418c5ecff0703da4b9bf01f93cceef4c38294610e4b543fbdc15370522ab3', N'0', N'2016-12-28 16:02:11.107')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'658', N'101', N'75bca56c9a8f79425c5f10d9fb6510809cc0b8866738e02b1864dca52c129ced', N'0', N'2017-03-24 16:38:12.440')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'81', N'101', N'767f44cd9a87713f9ace6c36e1000e35bee3a600c78214c942f91a7c76662a39', N'0', N'2016-12-27 10:11:45.943')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'137', N'101', N'770cd3c35e8ea5d6cc6b87dd2b04633c80bf681e299496476d6846ca017ea1cd', N'0', N'2016-12-30 16:59:03.637')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'603', N'101', N'7864e61e9fa3d467c6fd7a4f5d71075c6724d4329c6c7358d7508750ffcd9d28', N'0', N'2017-03-21 15:30:22.110')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'432', N'101', N'78e916e21ee39c106f39674b3f5764b78272539ceb1132e37d8c8e23b0263563', N'0', N'2017-02-26 15:26:18.570')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'85', N'101', N'792bb274bbd80eb73bebf268c206b96836c113d5d848d642db2de0bbc761cade', N'0', N'2016-12-27 10:12:16.027')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'626', N'101', N'7a5ab4c06ab04fe7a162ead78575204004cdd5f6cd55cd3e82a0b70778a6cc8d', N'0', N'2017-03-22 12:30:19.197')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'598', N'101', N'7adcfa3c942a9aefa86f929ee5b2c3b2d1b0db0820b3cde0b47d9aa3aa9f6b1e', N'0', N'2017-03-21 14:36:55.830')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'469', N'101', N'7b7492c3e8cac831c0b934753d023c127b744f6babc958def063d927611fc3c6', N'0', N'2017-03-03 18:19:55.597')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'408', N'101', N'7b7b33d27585dd5d068ec50326adcb081ad1a743d6797ba71ef89256a205072d', N'0', N'2017-02-23 11:03:32.613')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'500', N'101', N'7c3629bf9722ab5ffc0cc070a3cb743ff4dcf38efad9c2b78a210fd2059e855d', N'0', N'2017-03-09 11:07:00.547')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'481', N'101', N'7d5290842b7dfa3c71fe77c5a3dee5e4850607b1bfb25cf3fd67035474ec4354', N'0', N'2017-03-06 20:14:46.247')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'163', N'100', N'7d57e31eefbb45e217833c09cc64dabd30a9488adf72af5005742ab50d513e6d', N'0', N'2017-01-09 14:35:31.910')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'103', N'101', N'7d57e31eefbb45e217833c09cc64dabd30a9488adf72af5005742ab50d513e6d', N'0', N'2016-12-27 15:57:34.790')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'62', N'101', N'7d6bbac88935ad0cf1135d598c683c13c697c37fcfdcfda8035298b295d08d16', N'0', N'2016-12-23 22:38:17.990')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'100', N'101', N'7d8275cba61152f070581519aa36b8e03a302d2ccaa17b6c5fdd2303c34e9b70', N'0', N'2016-12-27 12:55:28.490')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'420', N'101', N'7dda60ff557da7fb2df9321977d10530003c1e25e52c594ccbee521edffe5c7d', N'0', N'2017-02-23 16:14:43.090')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'508', N'101', N'7e1617150afe775476bbf3b4e490ef248a56d860d2752d6f8c29bc10110fa343', N'0', N'2017-03-10 11:41:33.280')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'617', N'101', N'804ccc7d1c3303a4e52c25d66dd42f3dfc411f75b97d2539da7068c63e7e7a3d', N'0', N'2017-03-21 23:45:30.060')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'505', N'101', N'80a2d6016185eb43ea19fb126298ba3536422e309091ea58f5300fb3568de2dd', N'0', N'2017-03-09 16:43:25.953')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'118', N'101', N'81f223a53c3951b89fe8a06b9f1738875da0f70ee2b04dfe1de48a53ccd1d8dc', N'0', N'2016-12-28 15:43:11.487')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'438', N'101', N'82484917f7cd59b3a626decb913fd46759fbb6dc3482ea34a49b52b87c7d4e25', N'0', N'2017-02-27 20:51:44.080')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'589', N'101', N'8297ab5198e323a8240f084356ea6313052d8bda2f7485bc276e6e0cd0daf886', N'0', N'2017-03-20 17:44:41.140')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'378', N'101', N'829f2ff8ea738f0b1c107bcaf3659788ac5989ec2ac817e895421d5ec9a1ce66', N'0', N'2017-02-20 15:31:34.300')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'456', N'101', N'836f31354f17c85b300de3667f51ba100c0d02dbbd9a0916b2b275e8191cdbe1', N'0', N'2017-03-01 14:21:51.010')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'571', N'101', N'837b4971b510291a2d58451662b43b6a280148161c86bd811d239b5392f0c842', N'0', N'2017-03-17 18:25:45.323')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'406', N'101', N'83c9183db6aab77931eb4397a34659e86fb991dd0a922054958e8d3114b61f09', N'0', N'2017-02-23 10:29:39.060')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'538', N'101', N'846d87b10e5b5d9d3dcbe162dd97251b01e1af394709a0dffc2a1ef75c1d0d2c', N'0', N'2017-03-15 12:21:22.980')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'458', N'101', N'84d2fd0164dc014c88acba144e2a13771be34414f83f60d78558fdb2a4ddfac5', N'0', N'2017-03-01 14:52:12.417')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'531', N'101', N'852a5b0b63be85b375973e92ddc07cc12291807bbe09607a893d12277f44d58b', N'0', N'2017-03-14 19:54:54.980')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'588', N'101', N'85b25f1fe93f1e99e9e6270ed696da597279597dbf1cc8b8c22a4904dc8da176', N'0', N'2017-03-20 16:58:16.170')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'181', N'101', N'87176e65950c3886cefe87fe4acf7b67a47e2704c3dc70ce05aa00ba98f13829', N'0', N'2017-01-13 14:04:28.627')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'551', N'101', N'8776bc2188fa889a95ac0ab738675b936eaa050ae2614a547db7a44da8ded3e4', N'0', N'2017-03-16 16:35:22.610')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'460', N'101', N'87a97507f594c6cf7dacaff55ecbb35a630444a24b929a36c3f4e7b8b6d6a66f', N'0', N'2017-03-01 16:59:02.270')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'354', N'101', N'87e0ed66f537287b5552b27806c84edc287097371fb411c6fe97b55566276272', N'0', N'2017-02-15 23:28:11.467')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'582', N'101', N'87fd00cc0d36f21ff7224709e7bdffb7cf949f6af7c5686059303e3d6608a968', N'0', N'2017-03-20 11:09:10.623')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'430', N'101', N'890565c6ced546d4684d7858b8cd5af44e6befcbd80b419dda2f50b508e10973', N'0', N'2017-02-25 22:57:49.570')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'479', N'101', N'890727587324430ca282c209c6f58030876983df2fe62f41cb96dda750fae7a2', N'0', N'2017-03-06 17:20:18.173')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'569', N'101', N'8990347bb7d2e92dda4c4ef3a83dec08c863de67beebbaf4a20ec61c52c16968', N'0', N'2017-03-17 17:45:43.213')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'679', N'101', N'89f506b8192be615334f91fc7e3602981c046563f729ed8c5971bd439b34a510', N'0', N'2017-03-28 17:00:43.683')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'121', N'101', N'8a4b63aaf31d975fd9fd1aa1df4849a3a4648c3bb2dce1bb5f53fdbc1fa590ca', N'0', N'2016-12-28 16:16:43.797')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'405', N'101', N'8a71d0cd204c340de6cd58ad6227dc2a26d42fd1e77dcff1a19b18aedb5f2259', N'0', N'2017-02-23 10:03:30.713')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'328', N'101', N'8ae475fe9610698886aeec33ca2a31242d8138decee62c3f61fd16f10ccbaf1c', N'0', N'2017-02-13 10:29:33.920')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'501', N'101', N'8b20c31c0089e49700bf67e675cf5e44aae51b3d82f86715658a99430e1b5bef', N'0', N'2017-03-09 11:37:21.477')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'604', N'101', N'8bfdb2ee84233f54695e48d6a3232509ef1e0c7a3798c2047783d5de05e9d6bd', N'0', N'2017-03-21 15:44:50.370')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'502', N'101', N'8cfb3a27eafad1c77d50944d8a4178c5a6fae47d8a9e5fae14247b5e69d7deab', N'0', N'2017-03-09 11:40:48.050')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'549', N'101', N'8d83774a649faa9aafa67a6911f51a53bf162e39f9e4b542303d1e390b5d858d', N'0', N'2017-03-16 12:17:48.740')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'496', N'101', N'8da5ab7b248ac9391bc365324b105ee09fdc5f877db0aabf580217326ae35159', N'0', N'2017-03-08 14:54:53.283')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'244', N'100', N'8dca9545fc52a7c94f3a4ec467b066656451f7e720a8c97b114e8ec21559c096', N'0', N'2017-01-23 15:51:37.950')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'54', N'101', N'8dca9545fc52a7c94f3a4ec467b066656451f7e720a8c97b114e8ec21559c096', N'0', N'2016-12-22 14:55:56.347')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'178', N'101', N'8df48251b3bcbdc51ab58cae8ff24692f16c2efd6c612103c571eec70f45a45b', N'0', N'2017-01-12 19:37:17.270')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'665', N'101', N'8eb6803b114df9221fbe8834cb154518d4bb82ab3d7dcc354dfa901acc9fba82', N'0', N'2017-03-27 12:02:38.813')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'495', N'101', N'8f0a18c7c7add2d507f30c2a92738ce53ad8a6ea092832183b90bd37670b4578', N'0', N'2017-03-08 12:22:44.403')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'579', N'101', N'8fb50a22679d289bf4237970cc5b9dcb55fabdf20564d47101941f0c59e078e6', N'0', N'2017-03-18 16:11:25.043')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'389', N'101', N'90d423f5a3010c1a8fb3e86cd1ef1575c6327e7abb27c8fa4467f6641395cd7e', N'0', N'2017-02-20 19:25:40.757')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'411', N'101', N'91bec5b282baf9716bb6bf5f012a73e7a87e5b0aff198b3fad69f3029b6b1ca3', N'0', N'2017-02-23 11:17:11.107')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'294', N'101', N'9213afce52ccc003ba1be05a6746a199fc262a6c2d0739991c86fbb2cf09aa63', N'0', N'2017-02-08 17:56:14.173')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'452', N'101', N'9220e7c87d3bc317fd39200a208add07bf209d0437bb70134c29fef15e4722f8', N'0', N'2017-03-01 11:59:31.503')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'382', N'101', N'92d2fd14e87d1869cc23e32f0aaa6de239682806a17ed0ddefb15652ead70f78', N'0', N'2017-02-20 17:44:05.143')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'147', N'101', N'9408679f7f1afa8a933ef5b434000b393123a87d28bf3bc1da7a5f464da62512', N'0', N'2017-01-04 18:37:19.373')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'79', N'101', N'941d48c7559e99bbd783fb97faa911a564d907a0cd808148d6b35b4a3a9b4ad4', N'0', N'2016-12-27 10:11:29.957')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'390', N'101', N'94c045a17327e0caf938b486b7f3cd4b434bb975fde1433fd64b08d75d8f4031', N'0', N'2017-02-21 10:32:18.113')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'535', N'101', N'958d74c2336101138769df0902657dd499be8c17f6b921a2e8d38ad186a16857', N'0', N'2017-03-15 11:45:11.787')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'521', N'101', N'9640871747b653afe12b9bfd9d8ea9137e78299cd9b591a8ffffc88c3cfceba7', N'0', N'2017-03-14 10:14:20.117')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'138', N'101', N'987ce8e9cda537799301b30625d45f5b9d3d724abe54fded81d4b141b847359d', N'0', N'2016-12-30 18:54:06.713')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'526', N'101', N'9890da7475e9f954d8dd1392d0818b1acfa002c8a3987ec21fb23f6e79ae558e', N'0', N'2017-03-14 12:04:07.577')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'200', N'101', N'98a08f8332f54a659526cfae498855b88a56d860d2752d6f8c29bc10110fa343', N'0', N'2017-01-17 16:12:18.770')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'343', N'101', N'9a5408412f74a0f54b41d6556cf9acbe41b5876ae57d3c04a0f45684dd9e670f', N'0', N'2017-02-15 14:28:56.567')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'299', N'101', N'9c05168f0cb4f77d803cc777119d0cf2f502c34b35740dd42201f29b8ca544cd', N'0', N'2017-02-09 11:46:38.487')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'439', N'101', N'9ca0bf143428682f6d6d92879046927f948b0ac6f6bc812f510fd2b9c460a9b2', N'0', N'2017-02-28 09:37:57.190')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'385', N'101', N'9e5ad9edc739f01e67c8a80975da180ed68450258fe3461551613ba7e1f4d552', N'0', N'2017-02-20 18:43:43.203')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'208', N'101', N'9ec64c9f3cf2f097f70dd52b17184b5f3261d6c7b59e4e479b2e1beb613750fe', N'0', N'2017-01-17 17:32:32.273')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'230', N'101', N'9ecd7e7cb669460eb197687d0ac82641ccd6620dd2aab33c10461115fe124115', N'0', N'2017-01-20 14:44:52.380')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'189', N'101', N'9ece608edad2f22e25d860b0ef5cf78e49d87e80cdc6e59287a473ae5ea62170', N'0', N'2017-01-16 20:15:46.227')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'271', N'101', N'9f1f8f3550e6ff347f930b9cca961b60b3395c98ae6987ca70a651753fa880e9', N'0', N'2017-02-06 17:09:26.860')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'570', N'101', N'9f416297554347367a3bb01bcbd9a90820225533df4ce9d0497ad47aad12d824', N'0', N'2017-03-17 18:24:12.370')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'40', N'101', N'9f6d45ae087e58ca2ec6a73a62cde902d42c5aae4facd4761685eee12ee775d3', N'0', N'2016-12-21 19:31:58.217')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'593', N'101', N'a062beb09b91cfff2408153fbf2c52cb434dabfcac609fdf1530ba50154f6441', N'0', N'2017-03-20 21:02:41.260')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'402', N'101', N'a08c339c745d7137abf371912a3024c58a56d860d2752d6f8c29bc10110fa343', N'0', N'2017-02-22 16:37:17.357')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'193', N'101', N'a0ce243cf8bd1260b8e684a284be0dc88a56d860d2752d6f8c29bc10110fa343', N'0', N'2017-01-17 12:11:21.610')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'92', N'101', N'a0f14bd56755a400c37845a4630bd9af58a044727b3b4b5ef6c2922ffeaded87', N'0', N'2016-12-27 10:14:55.173')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'543', N'101', N'a213b1a2557f77ba0148b814fb70545301e656bf4b605f0f35d2cfa441856574', N'0', N'2017-03-15 15:04:57.487')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'635', N'101', N'a263046600ebbac8c162dc8b9e04f6e1207ec085fe40d6523afd9b7c26cb66e6', N'0', N'2017-03-22 18:27:30.370')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'82', N'101', N'a2fa71c4b90a1f49f44d762e24e58ced473a784208c2304a2235e57899a5817f', N'0', N'2016-12-27 10:11:47.223')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'670', N'101', N'a45598c482308f2fdd22931b30efa3843ac7b6a2bc978654d2832e3b8646bb78', N'0', N'2017-03-27 15:44:52.357')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'332', N'101', N'a56380740d75b8180d1a1eb13c5b7d30de15d7fd90e3fe988e12cf6c4a7976ba', N'0', N'2017-02-13 17:10:52.767')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'373', N'101', N'a5acf138422e192ef674dabb35d2452307bcd6d046565d27b853fa5a5cf07de6', N'0', N'2017-02-18 20:50:03.757')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'125', N'101', N'a645498723753b1fce7bdc86eedf1c98c38b61358a1b99011ba61c7088b2770c', N'0', N'2016-12-28 20:13:33.917')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'201', N'101', N'a6468f7cd022c7b4c2a617ff32e7748d9d5f0122a12708c1c2b5b298a22fe7fe', N'0', N'2017-01-17 16:16:36.660')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'542', N'101', N'a733ea6114627683583cd98767be30550d4308399091742d66d3b9c9e7392542', N'0', N'2017-03-15 14:59:10.093')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'144', N'101', N'a7628da769f3ec6e6ccf75be4d4f13c739772ec635665f1acfb50101cfc29b88', N'0', N'2017-01-03 17:32:19.423')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'673', N'101', N'a7ddce515f3edc5e140f53f6b0085a38a454807dbfbfd000e779aabbf1268411', N'0', N'2017-03-28 12:19:30.727')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'98', N'101', N'a7e3f903a871f4af56e549a201a42060a67aad03c40a6dddd031237fb02a141d', N'0', N'2016-12-27 12:10:29.360')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'524', N'101', N'a8245216d52b832b4685d2fb5e64c774ea1dd529ce2ffce52e18b43792988c6c', N'0', N'2017-03-14 10:56:13.307')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'565', N'101', N'a86ac5cf501cdcabbf0624c756810106a0c85a1aef260e1c969063756f097093', N'0', N'2017-03-17 10:53:14.787')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'231', N'101', N'a86c7e88efd5a8de4ab61d2d9f03ebc03be87bd87497699691180ca32550d339', N'0', N'2017-01-20 15:18:13.393')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'279', N'101', N'a8a465757af1db4b192bae7eb72b48b2c33027d222b82a884631ae0aef273fb1', N'0', N'2017-02-07 18:38:44.627')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'257', N'100', N'aa9244ec196b037b74e22e52af44d3ed0222846f0c1a698884d3ca8c063c6541', N'0', N'2017-02-04 10:54:37.973')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'601', N'100', N'aa9244ec196b037b74e22e52af44d3ed02630cfbe694b5da0ed4715c3b96788d', N'0', N'2017-03-21 15:09:24.187')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'366', N'100', N'aa9244ec196b037b74e22e52af44d3ed02f72873f6d8ba369988a3f93b553842', N'0', N'2017-02-18 16:33:03.370')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'65', N'100', N'aa9244ec196b037b74e22e52af44d3ed036afe166f31f029150900192dd06b53', N'0', N'2016-12-26 09:38:03.913')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'72', N'100', N'aa9244ec196b037b74e22e52af44d3ed0506e7d2bffa7cfeb340c7a2a298c269', N'0', N'2016-12-26 20:26:02.690')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'21', N'100', N'aa9244ec196b037b74e22e52af44d3ed0721fc1dc822831b8913914132b70ab7', N'0', N'2016-12-12 17:39:36.660')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'429', N'100', N'aa9244ec196b037b74e22e52af44d3ed083e9fc89449ccf57fd21bd5a5209ba9', N'0', N'2017-02-25 15:32:53.027')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'35', N'100', N'aa9244ec196b037b74e22e52af44d3ed08b3fa546b43753601251d1bec08fe41', N'0', N'2016-12-20 16:40:23.323')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'91', N'100', N'aa9244ec196b037b74e22e52af44d3ed08ed444df976976a01822aebb9cd5d3f', N'0', N'2016-12-27 10:14:43.353')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'127', N'100', N'aa9244ec196b037b74e22e52af44d3ed0965085e34121c7e546fbf73fa945cab', N'0', N'2016-12-29 09:43:01.497')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'22', N'100', N'aa9244ec196b037b74e22e52af44d3ed0a6fe463920a29ce9feae568cac667da', N'0', N'2016-12-12 19:41:12.487')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'228', N'100', N'aa9244ec196b037b74e22e52af44d3ed0ab7c8a80f3422bd5b04fbd2c6bd8d58', N'0', N'2017-01-19 20:26:52.163')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'625', N'100', N'aa9244ec196b037b74e22e52af44d3ed0bd28d6c079c88f208540441892d870f', N'0', N'2017-03-22 12:26:33.760')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'33', N'100', N'aa9244ec196b037b74e22e52af44d3ed0f8a3887b81ea8848bb492a3302a486e', N'0', N'2016-12-20 10:25:04.577')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'71', N'100', N'aa9244ec196b037b74e22e52af44d3ed1096d3efce7ef3ada4ecc1bc86331d4f', N'0', N'2016-12-26 19:52:00.090')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'113', N'101', N'aa9244ec196b037b74e22e52af44d3ed1096d3efce7ef3ada4ecc1bc86331d4f', N'0', N'2016-12-28 10:52:36.890')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'351', N'100', N'aa9244ec196b037b74e22e52af44d3ed143047ab7723a880e424d29c2924ffa0', N'0', N'2017-02-15 17:03:37.830')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'26', N'100', N'aa9244ec196b037b74e22e52af44d3ed16db00adecf8132b6afc57ad4fe8469c', N'0', N'2016-12-14 16:16:21.430')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'202', N'100', N'aa9244ec196b037b74e22e52af44d3ed177d7f3e4f863b5883b9853fa90df253', N'0', N'2017-01-17 16:26:57.063')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'88', N'100', N'aa9244ec196b037b74e22e52af44d3ed198e9145a3abdc7f62de1ee4b1e6c0d7', N'0', N'2016-12-27 10:13:42.367')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'615', N'100', N'aa9244ec196b037b74e22e52af44d3ed19a6b75ac871b626ab19754d7fb9f33a', N'0', N'2017-03-21 18:46:17.660')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'397', N'100', N'aa9244ec196b037b74e22e52af44d3ed1f8bc42733b81b5584ee4b33bd748153', N'0', N'2017-02-21 18:20:23.173')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'8', N'100', N'aa9244ec196b037b74e22e52af44d3ed218fd359b5ff8e2dad6da515403facef', N'0', N'2016-11-28 19:10:14.420')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'90', N'100', N'aa9244ec196b037b74e22e52af44d3ed23ffbd4c9bb89477c86374b7e481025b', N'0', N'2016-12-27 10:13:46.883')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'358', N'100', N'aa9244ec196b037b74e22e52af44d3ed24f7187b98719356756814b2c0060510', N'0', N'2017-02-16 19:33:16.993')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'398', N'100', N'aa9244ec196b037b74e22e52af44d3ed25116ca661636fe89b42372cb669832e', N'0', N'2017-02-22 10:55:33.283')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'9', N'100', N'aa9244ec196b037b74e22e52af44d3ed27fab9be70973393c735352db77c66b6', N'0', N'2016-11-28 19:18:19.520')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'262', N'100', N'aa9244ec196b037b74e22e52af44d3ed2a1057b1cff369d33dbd2613830cdec0', N'0', N'2017-02-06 14:41:44.727')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'586', N'100', N'aa9244ec196b037b74e22e52af44d3ed2ba8303c892d03a924904389edccaa40', N'0', N'2017-03-20 13:49:52.773')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'87', N'100', N'aa9244ec196b037b74e22e52af44d3ed2ccccc7a5bf9c73f05d7afe8512f39d1', N'0', N'2016-12-27 10:13:39.993')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'124', N'100', N'aa9244ec196b037b74e22e52af44d3ed2fd6aca34317366897e8d17c6f86fc5d', N'0', N'2016-12-28 19:55:52.033')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'235', N'100', N'aa9244ec196b037b74e22e52af44d3ed31325f79ce34d4f1a58d8cd0262586a0', N'0', N'2017-01-22 17:15:36.423')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'512', N'101', N'aa9244ec196b037b74e22e52af44d3ed31325f79ce34d4f1a58d8cd0262586a0', N'0', N'2017-03-10 11:54:06.047')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'12', N'100', N'aa9244ec196b037b74e22e52af44d3ed334ede9b93b56f99ee4ad5e3a8a74465', N'0', N'2016-12-01 09:51:50.467')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'585', N'100', N'aa9244ec196b037b74e22e52af44d3ed35506930a69fe694f225fcefa9ce98e5', N'0', N'2017-03-20 13:47:57.087')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'53', N'100', N'aa9244ec196b037b74e22e52af44d3ed38f6faf14512d1ddcca2cc71ef09f41d', N'0', N'2016-12-22 14:54:08.913')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'342', N'100', N'aa9244ec196b037b74e22e52af44d3ed394521bcffcaef8930452fb7636ec18c', N'0', N'2017-02-15 13:51:52.623')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'473', N'100', N'aa9244ec196b037b74e22e52af44d3ed394b1d6c40f714886483b1994c3ccf2e', N'0', N'2017-03-04 19:03:11.590')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'128', N'100', N'aa9244ec196b037b74e22e52af44d3ed3962b11869a1e47a3c22a3360fd3d8bf', N'0', N'2016-12-29 17:18:30.817')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'57', N'100', N'aa9244ec196b037b74e22e52af44d3ed3a8889ab0dba8562ad8e1789b27531a7', N'0', N'2016-12-22 16:03:26.250')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'306', N'100', N'aa9244ec196b037b74e22e52af44d3ed3d384f742a87b84f0db638c87027d74e', N'0', N'2017-02-09 16:08:10.490')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'50', N'100', N'aa9244ec196b037b74e22e52af44d3ed3e92786455d1bc1f0581eec6e719985e', N'0', N'2016-12-22 12:59:20.437')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'237', N'100', N'aa9244ec196b037b74e22e52af44d3ed3edc50dd2396921a908931289d71f28b', N'0', N'2017-01-22 22:19:23.043')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'69', N'100', N'aa9244ec196b037b74e22e52af44d3ed3f4c155918ca3e2d348dbdefe346978e', N'0', N'2016-12-26 15:56:57.673')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'19', N'100', N'aa9244ec196b037b74e22e52af44d3ed427b712a58d78c72af2ab726e535c365', N'0', N'2016-12-08 14:43:11.267')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'58', N'100', N'aa9244ec196b037b74e22e52af44d3ed468243161a54c7849067ad556f722e1d', N'0', N'2016-12-23 17:02:26.803')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'188', N'100', N'aa9244ec196b037b74e22e52af44d3ed47343a4c5cc45fbe949371943e7efccb', N'0', N'2017-01-16 20:15:16.543')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'18', N'100', N'aa9244ec196b037b74e22e52af44d3ed49e468cf25bbe169c866f0415ae79778', N'0', N'2016-12-08 14:11:05.457')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'95', N'100', N'aa9244ec196b037b74e22e52af44d3ed4b92b4ce741ef506e67e18da130c83b6', N'0', N'2016-12-27 10:15:33.947')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'14', N'100', N'aa9244ec196b037b74e22e52af44d3ed4bb89d79f7fb4c9e7889745467dcf7f6', N'0', N'2016-12-01 16:09:39.070')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'349', N'100', N'aa9244ec196b037b74e22e52af44d3ed4c2153c9c84fa216d406985c2fc1b7e6', N'0', N'2017-02-15 16:42:39.027')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'641', N'100', N'aa9244ec196b037b74e22e52af44d3ed4c4d7674fc13fe76baa1b12c223eab6c', N'0', N'2017-03-23 10:32:02.847')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'49', N'100', N'aa9244ec196b037b74e22e52af44d3ed4c5370bf469516914fff695993bba90a', N'0', N'2016-12-22 12:35:50.983')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'55', N'101', N'aa9244ec196b037b74e22e52af44d3ed4c5370bf469516914fff695993bba90a', N'0', N'2016-12-22 15:40:36.060')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'248', N'100', N'aa9244ec196b037b74e22e52af44d3ed4f2e86fdd7b6573f713e8d0600f93d70', N'0', N'2017-01-23 17:57:06.550')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'16', N'100', N'aa9244ec196b037b74e22e52af44d3ed4fceae147267dcbdf9d0c846f2e60893', N'0', N'2016-12-03 16:09:44.487')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'232', N'100', N'aa9244ec196b037b74e22e52af44d3ed5345b52abb122e818f1323f3cbd3597b', N'0', N'2017-01-20 18:54:08.000')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'15', N'100', N'aa9244ec196b037b74e22e52af44d3ed54f39594ed27ded19449e7db5df9072d', N'0', N'2016-12-01 16:41:46.340')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'1', N'100', N'aa9244ec196b037b74e22e52af44d3ed554dc8038fdf3d6f02be4cc81f38144e', N'0', N'2016-11-25 13:50:55.743')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'215', N'101', N'aa9244ec196b037b74e22e52af44d3ed554dc8038fdf3d6f02be4cc81f38144e', N'0', N'2017-01-18 10:49:48.497')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'27', N'100', N'aa9244ec196b037b74e22e52af44d3ed5ac52b8751a864fd8288d0f2b1cfd481', N'0', N'2016-12-15 11:16:24.613')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'17', N'100', N'aa9244ec196b037b74e22e52af44d3ed5e8e01a8720f5a4db5fcad3a12026f54', N'0', N'2016-12-07 11:10:00.700')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'250', N'100', N'aa9244ec196b037b74e22e52af44d3ed615efb07b30bcbc3e70ffb24ff9684cd', N'0', N'2017-01-24 16:00:21.770')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'251', N'100', N'aa9244ec196b037b74e22e52af44d3ed61808bb76f82a41803057f616e164ed9', N'0', N'2017-01-25 14:17:26.213')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'30', N'100', N'aa9244ec196b037b74e22e52af44d3ed6270865119519afbba81e8183c5bdf2f', N'0', N'2016-12-16 16:01:09.043')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'309', N'100', N'aa9244ec196b037b74e22e52af44d3ed65c7ae8772755f5c06a430a493aeb58f', N'0', N'2017-02-09 16:22:17.263')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'600', N'100', N'aa9244ec196b037b74e22e52af44d3ed6a08ab2668e112326a6f3e46ad63609b', N'0', N'2017-03-21 15:07:10.207')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'649', N'100', N'aa9244ec196b037b74e22e52af44d3ed71dd969a2392087a6cd447c2eaf40b01', N'0', N'2017-03-23 16:59:01.843')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'275', N'100', N'aa9244ec196b037b74e22e52af44d3ed71e46d093b05b0b92cc91032ef98ee4e', N'0', N'2017-02-06 17:14:53.023')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'243', N'100', N'aa9244ec196b037b74e22e52af44d3ed746389d042792b033f82d5afadcc8a29', N'0', N'2017-01-23 15:50:34.367')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'677', N'100', N'aa9244ec196b037b74e22e52af44d3ed75c0bfe7608eba5d36131ecbb48c438f', N'0', N'2017-03-28 16:51:47.050')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'238', N'100', N'aa9244ec196b037b74e22e52af44d3ed76a74096781e1b0f46d503409a754a7a', N'0', N'2017-01-22 23:18:35.163')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'46', N'100', N'aa9244ec196b037b74e22e52af44d3ed7930a7dbb2b9b17049fb800cd5495a8d', N'0', N'2016-12-22 11:40:32.440')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'675', N'100', N'aa9244ec196b037b74e22e52af44d3ed7b9c513511f4e00912e9abad2aae5d83', N'0', N'2017-03-28 15:49:32.207')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'307', N'100', N'aa9244ec196b037b74e22e52af44d3ed7c5e805e485a5d2c9661b7c4947db10d', N'0', N'2017-02-09 16:11:00.500')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'611', N'100', N'aa9244ec196b037b74e22e52af44d3ed7cfa7513ef59d69ca634d124e4c42606', N'0', N'2017-03-21 16:55:10.143')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'662', N'100', N'aa9244ec196b037b74e22e52af44d3ed7e5f548ad025c5b0b0b55a3232d8d72d', N'0', N'2017-03-24 18:42:43.287')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'292', N'100', N'aa9244ec196b037b74e22e52af44d3ed7eb30774b06b527d7b4dd60d4b9f3e7a', N'0', N'2017-02-08 16:12:40.577')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'511', N'101', N'aa9244ec196b037b74e22e52af44d3ed7eb30774b06b527d7b4dd60d4b9f3e7a', N'0', N'2017-03-10 11:53:31.890')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'236', N'100', N'aa9244ec196b037b74e22e52af44d3ed7f439dec4f4c34f7df7a942dc85ed406', N'0', N'2017-01-22 17:46:32.720')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'234', N'100', N'aa9244ec196b037b74e22e52af44d3ed83442a889175e204672bc070e41ba26a', N'0', N'2017-01-22 17:02:58.757')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'247', N'100', N'aa9244ec196b037b74e22e52af44d3ed863128808120f3dab58723be09adfb08', N'0', N'2017-01-23 16:52:37.210')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'93', N'100', N'aa9244ec196b037b74e22e52af44d3ed8a783040f0a843f63b708db14832eeeb', N'0', N'2016-12-27 10:14:55.457')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'28', N'100', N'aa9244ec196b037b74e22e52af44d3ed8ba25f0eb85fd03fe6c8f4895b577889', N'0', N'2016-12-15 21:19:55.283')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'162', N'100', N'aa9244ec196b037b74e22e52af44d3ed8bf4a3be69712bab3938f8222a12d0b9', N'0', N'2017-01-09 14:33:17.547')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'226', N'100', N'aa9244ec196b037b74e22e52af44d3ed90f7e885f9c90297981a581eede14152', N'0', N'2017-01-19 15:10:21.660')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'510', N'101', N'aa9244ec196b037b74e22e52af44d3ed90f7e885f9c90297981a581eede14152', N'0', N'2017-03-10 11:51:38.263')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'345', N'100', N'aa9244ec196b037b74e22e52af44d3ed91c1b059431a4e8c9a44aa38560ff833', N'0', N'2017-02-15 15:25:27.110')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'295', N'100', N'aa9244ec196b037b74e22e52af44d3ed91f901ffb736c836c8c8110f202714e7', N'0', N'2017-02-08 18:29:37.370')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'29', N'100', N'aa9244ec196b037b74e22e52af44d3ed92b5f0b8a3d469c8409731ac69117b64', N'0', N'2016-12-16 13:35:28.973')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'7', N'100', N'aa9244ec196b037b74e22e52af44d3ed94dde56165f13ac9067e1da0359b94d9', N'0', N'2016-11-28 14:43:58.283')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'2', N'100', N'aa9244ec196b037b74e22e52af44d3ed970a965cd43aa85e6034f70eca6f5ced', N'0', N'2016-11-25 15:29:42.603')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'216', N'101', N'aa9244ec196b037b74e22e52af44d3ed970a965cd43aa85e6034f70eca6f5ced', N'0', N'2017-01-18 10:57:49.653')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'20', N'100', N'aa9244ec196b037b74e22e52af44d3ed98fdba0f4bb608436e2d1bd82534a3ac', N'0', N'2016-12-09 14:21:18.587')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'25', N'100', N'aa9244ec196b037b74e22e52af44d3ed9bd2c03c41a9a59f0753dfdfb527e26d', N'0', N'2016-12-13 18:56:58.193')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'229', N'100', N'aa9244ec196b037b74e22e52af44d3ed9d3d5caa5b126fef6d2d124db1c3c787', N'0', N'2017-01-19 21:24:03.383')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'610', N'100', N'aa9244ec196b037b74e22e52af44d3ed9f77a6462ae54d41434ad3bfc2384423', N'0', N'2017-03-21 16:54:56.220')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'314', N'100', N'aa9244ec196b037b74e22e52af44d3eda05be2b3af72a8d9a4e3ddd2098808e1', N'0', N'2017-02-09 21:10:01.613')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'96', N'100', N'aa9244ec196b037b74e22e52af44d3eda3f130c6c99d349fdd18d030bc913d2a', N'0', N'2016-12-27 10:16:15.183')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'233', N'100', N'aa9244ec196b037b74e22e52af44d3eda458e5276a1562f810851198defe381a', N'0', N'2017-01-22 11:11:57.710')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'528', N'100', N'aa9244ec196b037b74e22e52af44d3eda5ceef4bfbd914ec320b17b942039d7c', N'0', N'2017-03-14 15:09:45.717')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'52', N'100', N'aa9244ec196b037b74e22e52af44d3edafd4c7aec49a55888979b4226379ea77', N'0', N'2016-12-22 14:08:37.357')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'10', N'100', N'aa9244ec196b037b74e22e52af44d3edb2bd8ab6eb2bc696ffed67363adfb884', N'0', N'2016-11-28 21:14:14.900')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'36', N'100', N'aa9244ec196b037b74e22e52af44d3edb4cbabf7eb088ed73ec0e065afa38dbb', N'0', N'2016-12-20 18:08:55.043')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'631', N'100', N'aa9244ec196b037b74e22e52af44d3edb5e7d0e2802b37ab2586a27d0804620f', N'0', N'2017-03-22 15:51:32.740')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'3', N'100', N'aa9244ec196b037b74e22e52af44d3edb61ef3bcf4e76cb7988e5e19d3e225aa', N'0', N'2016-11-28 09:14:30.223')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'225', N'100', N'aa9244ec196b037b74e22e52af44d3edb674a75bf6e60993e5deb46b53ba30a9', N'0', N'2017-01-19 13:58:00.010')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'422', N'100', N'aa9244ec196b037b74e22e52af44d3edb7382181f5c86383f43459a7167fb988', N'0', N'2017-02-23 17:18:37.617')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'391', N'100', N'aa9244ec196b037b74e22e52af44d3edb802673f3a85e241b29d934aa615db1b', N'0', N'2017-02-21 14:17:14.550')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'529', N'100', N'aa9244ec196b037b74e22e52af44d3edb8b39b0226def150370ac965ede19a47', N'0', N'2017-03-14 15:43:36.160')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'474', N'100', N'aa9244ec196b037b74e22e52af44d3edb9292fc259c4679a7ec53cbf76c1e480', N'0', N'2017-03-04 19:03:13.190')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'214', N'100', N'aa9244ec196b037b74e22e52af44d3edb9a72ec9ce5144ded5eddf48c0e1572c', N'0', N'2017-01-18 10:26:58.080')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'276', N'100', N'aa9244ec196b037b74e22e52af44d3edb9d2602fb3e8fc943ce21ee04a918757', N'0', N'2017-02-06 17:21:13.687')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'348', N'100', N'aa9244ec196b037b74e22e52af44d3edbb8ec1d54e1008a8daf3f61a20c6d29e', N'0', N'2017-02-15 16:37:52.800')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'676', N'100', N'aa9244ec196b037b74e22e52af44d3edbbb0491091c7eb1d443da51af2fb1d50', N'0', N'2017-03-28 15:55:49.480')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'396', N'100', N'aa9244ec196b037b74e22e52af44d3edbc2e0d2806839139ad384d19c7b6f025', N'0', N'2017-02-21 18:06:30.437')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'73', N'100', N'aa9244ec196b037b74e22e52af44d3edbedede25d5bd8e016656e9152a8e2415', N'0', N'2016-12-26 20:46:23.337')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'186', N'100', N'aa9244ec196b037b74e22e52af44d3edbfd3ff10ca2d5a2d1a74cf17e053de0c', N'0', N'2017-01-16 15:11:07.547')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'43', N'100', N'aa9244ec196b037b74e22e52af44d3edc2f78ac8d83ee7c5e012081afd27cf25', N'0', N'2016-12-22 09:41:34.747')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'678', N'100', N'aa9244ec196b037b74e22e52af44d3edc35a20f737644063069df9cb5fc51537', N'0', N'2017-03-28 16:58:16.090')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'106', N'100', N'aa9244ec196b037b74e22e52af44d3edc44f62ea1b2194a1e752a59c61017c54', N'0', N'2016-12-27 20:08:42.403')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'112', N'101', N'aa9244ec196b037b74e22e52af44d3edc44f62ea1b2194a1e752a59c61017c54', N'0', N'2016-12-28 10:50:10.020')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'6', N'100', N'aa9244ec196b037b74e22e52af44d3edc47aad1ccf1e846e918f95a4dcd7a9ce', N'0', N'2016-11-28 12:46:51.843')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'290', N'100', N'aa9244ec196b037b74e22e52af44d3edc543dd054e9c8a354771a3f21b9c18c6', N'0', N'2017-02-08 15:30:34.853')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'31', N'100', N'aa9244ec196b037b74e22e52af44d3edc63dc41e8c7f1012301e78a54ded71e9', N'0', N'2016-12-16 19:34:00.393')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'548', N'100', N'aa9244ec196b037b74e22e52af44d3edc7a8080ecf8c51a6de099f34b6f0dde0', N'0', N'2017-03-16 11:45:41.880')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'24', N'100', N'aa9244ec196b037b74e22e52af44d3edc8a595452c8b75299c540cd1e744a5d6', N'0', N'2016-12-13 18:56:24.990')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'34', N'100', N'aa9244ec196b037b74e22e52af44d3edca04c626dee8bb5c39509eecaf42d6af', N'0', N'2016-12-20 14:58:29.873')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'353', N'100', N'aa9244ec196b037b74e22e52af44d3edcadc208ee8a6bc1344a51f0875b0a91f', N'0', N'2017-02-15 18:27:32.500')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'513', N'101', N'aa9244ec196b037b74e22e52af44d3edcadc208ee8a6bc1344a51f0875b0a91f', N'0', N'2017-03-10 11:54:45.260')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'23', N'100', N'aa9244ec196b037b74e22e52af44d3edcbcb2d91a0923f9f524dd4827f16cab3', N'0', N'2016-12-13 11:25:25.300')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'217', N'101', N'aa9244ec196b037b74e22e52af44d3edcbcb2d91a0923f9f524dd4827f16cab3', N'0', N'2017-01-18 11:07:21.997')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'350', N'100', N'aa9244ec196b037b74e22e52af44d3edcbe938c100b95a882e65ce20e1a4a27b', N'0', N'2017-02-15 17:02:35.323')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'56', N'100', N'aa9244ec196b037b74e22e52af44d3edcbf63ecc3a60e6102a360d89e1dae9aa', N'0', N'2016-12-22 15:48:13.167')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'32', N'100', N'aa9244ec196b037b74e22e52af44d3edcd166ba1a0f45686a5c7211e6ff76116', N'0', N'2016-12-16 19:35:04.100')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'11', N'100', N'aa9244ec196b037b74e22e52af44d3edcf478e305b3d52f424434da79eae5e7b', N'0', N'2016-11-29 13:06:50.453')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'187', N'100', N'aa9244ec196b037b74e22e52af44d3edcf59e228685d6a22e2771fef2b9df000', N'0', N'2017-01-16 15:30:49.130')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'101', N'100', N'aa9244ec196b037b74e22e52af44d3edd165d4dde9cf28f6541b52afbc72bebb', N'0', N'2016-12-27 14:18:34.673')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'129', N'101', N'aa9244ec196b037b74e22e52af44d3edd165d4dde9cf28f6541b52afbc72bebb', N'0', N'2016-12-29 20:40:01.777')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'4', N'100', N'aa9244ec196b037b74e22e52af44d3edd5e32be65f1d4d63de1f557c198e09a5', N'0', N'2016-11-28 09:58:15.270')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'259', N'100', N'aa9244ec196b037b74e22e52af44d3edd9b56a2d7cca8160f5f6e76275148196', N'0', N'2017-02-04 18:16:27.290')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'13', N'100', N'aa9244ec196b037b74e22e52af44d3eddf0ea3ea9e6f33f554b97321158068c9', N'0', N'2016-12-01 09:56:20.960')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'671', N'100', N'aa9244ec196b037b74e22e52af44d3ede1d65855fb546db4735077e646bef80f', N'0', N'2017-03-27 17:25:24.110')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'313', N'100', N'aa9244ec196b037b74e22e52af44d3eded8aa071278058315b2a8e33e68ec049', N'0', N'2017-02-09 20:43:59.213')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'310', N'100', N'aa9244ec196b037b74e22e52af44d3edef281be0c6b70064d8177f71432b68ce', N'0', N'2017-02-09 16:23:51.533')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'5', N'100', N'aa9244ec196b037b74e22e52af44d3edef5db07b81ec2f474ed8b34bd932218f', N'0', N'2016-11-28 10:17:33.267')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'89', N'100', N'aa9244ec196b037b74e22e52af44d3edf125a72f636aa9fdff74048c2f1964c4', N'0', N'2016-12-27 10:13:43.713')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'94', N'100', N'aa9244ec196b037b74e22e52af44d3edf484424c3d1192714af9b3ab37c059fe', N'0', N'2016-12-27 10:15:04.497')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'596', N'100', N'aa9244ec196b037b74e22e52af44d3edf4d8136bbbcd18730a285d46b7298fdd', N'0', N'2017-03-21 11:19:14.120')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'346', N'100', N'aa9244ec196b037b74e22e52af44d3edfc9b5823765d83533f58782ffb345b00', N'0', N'2017-02-15 15:30:41.433')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'37', N'100', N'aa9244ec196b037b74e22e52af44d3edfd54e5130ad400c1273680468d1d2d6f', N'0', N'2016-12-21 18:15:54.193')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'381', N'101', N'ab24f9913a2d8dec8d9aaa2baed8c36a58d62d8fdeedb93ec68a69321de9159e', N'0', N'2017-02-20 17:36:37.600')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'107', N'101', N'ab5fbcee5d9cbae891525acc10a1d24d838f46faf9a5cc1ba86d68b5ab61eb70', N'0', N'2016-12-27 22:42:10.770')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'441', N'101', N'abd3159275a3904b1ee3f2acc30435edbe59152dd02e39ef3cc78b7aea530d5f', N'0', N'2017-02-28 10:24:47.317')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'115', N'101', N'ac1fd4c05c6e3e86d3269346d39858250dd402efb2d3b3d8992adddf2b7bf5eb', N'0', N'2016-12-28 11:05:06.463')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'288', N'101', N'add394540ce4b36093af3d70c2e8093c6aa0309392651112d29b3478333082d6', N'0', N'2017-02-08 15:00:20.893')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'298', N'101', N'ae705c5bb688eb968ca08d7144a90e104282baed3f94d5bd02fecaedf9c14f51', N'0', N'2017-02-09 11:40:59.670')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'423', N'101', N'ae9404749278de556ef8e3c10ac3859401c8b5f3c121421f04476d567e473a10', N'0', N'2017-02-23 17:42:33.730')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'99', N'101', N'aed1c69669aa5c01089361e1ac8a7e4c8bcb76f46d9e6119b1bbdc7ad855a0c7', N'0', N'2016-12-27 12:26:44.230')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'329', N'101', N'af2bff84df5d286e9f87abf2f2cc326c5d7cc9c350e8d4ca9b9c70f5fe4dab52', N'0', N'2017-02-13 10:58:40.770')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'461', N'101', N'af77b53f0cc43a81cf85a762c433189c9a88c1134edef6d000ea568f59b26369', N'0', N'2017-03-02 11:19:32.153')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'371', N'101', N'afb8d48490f0df8feb37ba337ae5cf25a500b68462b49bf6bd07b843f33d26df', N'0', N'2017-02-18 19:43:16.273')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'153', N'101', N'afc7f2d2a222fb3a4b88e05e74259db8eda59a37a24d9563a37ec012aa5cf0bb', N'0', N'2017-01-05 16:44:38.410')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'436', N'101', N'b04be18be2c59996aadc9aacb2b706b784eb6404e62641a076a6b1f93a7b2db4', N'0', N'2017-02-27 11:42:02.570')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'205', N'101', N'b0d4b4cd1e16707cded9daa2fc852d0c9b9031932a5d4d834a681c2b1537c622', N'0', N'2017-01-17 17:18:52.777')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'487', N'101', N'b10fd9750a9b08df691be489b36e2d133c2cd7662b2b96bf09e8590ee9640938', N'0', N'2017-03-07 14:49:53.450')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'650', N'101', N'b12f77beed0709514f571d2d732e5e421953cc04efc05ddd68d9a13fc1f930d4', N'0', N'2017-03-23 19:23:18.193')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'484', N'101', N'b14b55812bcce07f12524729dbffd67760e91068ceafcb1bf4876092af7c3119', N'0', N'2017-03-07 10:20:25.083')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'564', N'101', N'b14d0719b5de8a2b97f3117845565ea94ec9770902aeb39c66f736f26d68151e', N'0', N'2017-03-17 10:52:39.173')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'642', N'101', N'b1fa1db489af7ccd711c3f4f918bd0d49a7d14ec1b2db3de07b6dec388259725', N'0', N'2017-03-23 11:34:19.900')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'108', N'101', N'b2e64d53c204a88064934f08ce1cedaeb685b121c817b24d37347e7d1b6bc733', N'0', N'2016-12-28 01:06:21.870')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'561', N'101', N'b2ea6b89613609cea23c1a422833044934bafebf3642117a8f81710acc4f6f9d', N'0', N'2017-03-17 10:16:43.853')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'457', N'101', N'b32b618a2987cc8ad54e0dea4b0dfe38fff8241a3bce6eb1d6d6802b9feada52', N'0', N'2017-03-01 14:52:10.993')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'395', N'101', N'b40213870e437be3a3d9f43afc39cad64d8a37e0090b077a0a1829540430cb9e', N'0', N'2017-02-21 18:02:18.850')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'305', N'101', N'b479d4b8ed86630b84e0a423722fbc5ba1dcce021802d071014c49ce0b176bc1', N'0', N'2017-02-09 16:05:16.900')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'171', N'101', N'b494d8e7d5fe9337e88ebd1e4871250d74d47c3c57606e91dcc478e0fcaeb14c', N'0', N'2017-01-11 11:00:32.280')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'431', N'101', N'b7784c8cb8e6f2bbfed5be3c5f9bf74fe2ef4d7b7f6f56ffa3bc941352f65cbf', N'0', N'2017-02-26 14:51:44.870')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'464', N'101', N'b779e21816a15172dc020c86e275381bb89d853187a6ee93b113edd02ba779f2', N'0', N'2017-03-03 16:34:24.047')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'466', N'101', N'b84e8d4bceacbe9264ef01130feaa0ca955fbbfd0a14bc78b80c97371ff9406d', N'0', N'2017-03-03 17:18:00.523')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'316', N'101', N'b89c6d01518be75610cd0d86ed6df1950bdc4c409c60e92b74fb11cf07fca12e', N'0', N'2017-02-10 14:53:09.987')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'324', N'101', N'b9d710de085dfc82ea9a3f1c2c576a8a9cf2654fc912c00e5e5b097680c5582d', N'0', N'2017-02-11 14:08:27.350')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'403', N'101', N'ba2c8136d262de762f351466faccc60671fb719a5d88c3660ab7ce7f2c2f0466', N'0', N'2017-02-22 18:46:28.213')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'136', N'101', N'ba68c8f1d9098e0e6c1eaf64f2a7bf6f18d001c16d97a6f6a337437309a102c3', N'0', N'2016-12-30 16:54:40.087')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'199', N'101', N'bb0eadf9adf9c6af99febdab7f897f766b73b6456a78afcaa0249c49b817569e', N'0', N'2017-01-17 15:59:40.070')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'659', N'101', N'bb67c9b9ce2d1eead5ea2748e19c531318fbc4fb04b0964c4762a6a4e880994f', N'0', N'2017-03-24 16:56:20.690')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'322', N'101', N'bb6c210b534a747a293c22f6a0fe809a903f1de49b2bfae9b56c386d5bbbad53', N'0', N'2017-02-10 23:00:25.867')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'654', N'101', N'bc2af881c5a4229eb88a9cf2ea8d2643b003d9f6be29245eb3fc82e1e0d42ac8', N'0', N'2017-03-24 10:29:15.943')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'76', N'101', N'bc5742b04389b8b8cd5e49906ce5c6fd11ef7af18c8d0c9e2c0feb12951da576', N'0', N'2016-12-27 10:09:59.320')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'80', N'101', N'bc7fb46c0050e78f1533ce0a42d0807002186e2d3d102cfa4aa4fbe6f429e677', N'0', N'2016-12-27 10:11:44.583')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'451', N'101', N'bcc4316ee59adce49b3204754716f5530a67c4ee81fa7ff52309e83c112900a3', N'0', N'2017-03-01 11:49:39.330')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'104', N'101', N'bd2490f97220aba51d5a78e5c4c1774fccc93e7e34c80ca679185816e68d3802', N'0', N'2016-12-27 16:13:32.017')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'119', N'101', N'bf0a0ce64bc6d51bc8272c3a4e57c66b58d5118e70ea96855bec74cb3bc86c3a', N'0', N'2016-12-28 15:58:36.590')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'195', N'101', N'bfe949b209ca9eb4e9b313370381b22b742505455d28e4382f2eec921b9f9c8d', N'0', N'2017-01-17 13:51:13.313')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'640', N'101', N'bfea4fa70775ade330f61a98949b83e7e93ee0b3077d5ad4058c87665d5dd7a0', N'0', N'2017-03-22 22:10:02.150')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'320', N'101', N'c05f9eae3a89482737318f981cb8df19070fe35b9ce32da52913194c5521c4d8', N'0', N'2017-02-10 16:29:52.900')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'599', N'101', N'c15102d8b666f1a9629bb3a6b0ff0b2f71ed415e6517c34e46a24aacdbcbb561', N'0', N'2017-03-21 14:38:17.063')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'302', N'101', N'c1970d1d9955660e7fbac8cd4743e4f105f4605564a46f0f4463af8db8804da1', N'0', N'2017-02-09 15:22:27.570')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'286', N'101', N'c1ac1bccfaa78db8ed6e8a49274e942976d92652190b617d40a865ec9cfa1140', N'0', N'2017-02-08 11:40:50.480')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'606', N'101', N'c2a3a53ecc69ae9fc9b13b716965ea78bae7699e35628b9ab7cbaffda5cb4927', N'0', N'2017-03-21 16:02:58.427')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'355', N'101', N'c2c6598b50bcef80c654fe319d686cb76239c00c0c0c2fcc8a97a6f3e8e1c5c1', N'0', N'2017-02-16 11:06:24.210')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'666', N'101', N'c2eba993b468e5242b435b1be09b22f08206f1263c2e9949b34dbe1ea1ce49dd', N'0', N'2017-03-27 12:03:51.117')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'184', N'101', N'c3ffc5fb88318569be88c10c05e0275f97054926e37a17c6157c335e400df7ec', N'0', N'2017-01-14 19:14:20.073')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'459', N'101', N'c4229d2381cea601da9018be1b1e287d70d40b0743d82ce42e1cc837327d8819', N'0', N'2017-03-01 15:04:35.867')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'105', N'101', N'c563aa72a85db6499b4605bc05824887e2e000ef0c84315a8d90856b3555a18b', N'0', N'2016-12-27 18:46:12.393')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'334', N'101', N'c5739c68c565eb86214872a6943233b8f42a74888cc337c7031ad14bec236d40', N'0', N'2017-02-13 23:52:19.140')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'283', N'101', N'c669d2244e9de1f73f79ffcb7b16b907af70662299e54e8686645ca774e6cf3e', N'0', N'2017-02-08 11:00:30.183')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'183', N'101', N'c6e4e24013ffb991149d9f255745a179c36c6fa52838d1c4859fd541bcacb457', N'0', N'2017-01-14 19:12:24.620')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'209', N'101', N'c7073d7ad258668e841550adda10f8d542b0b01dfe1307a19135d8d5b7c29c9b', N'0', N'2017-01-17 17:37:16.310')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'264', N'101', N'c7d277dc717174a52cc21824196a6ad809c0c7cb8c37825f9edbca7d835c7fac', N'0', N'2017-02-06 14:57:13.630')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'256', N'101', N'c81048247c565ca70cc89ec268bd866afaa7e0708072b79518c3e69d24f1dab3', N'0', N'2017-02-03 12:43:38.150')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'553', N'101', N'c97d10a29adca67495897e556206a81fb9b7465a5d1f1d365b5a14c125460914', N'0', N'2017-03-16 17:40:58.337')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'269', N'101', N'ca1953656a2001844a9d4a56a2640912b3395c98ae6987ca70a651753fa880e9', N'0', N'2017-02-06 15:54:51.870')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'326', N'101', N'ca8974bb7b06b629d4ec6bd2b982947a587e251acadf958b4f7068852bab765b', N'0', N'2017-02-12 19:17:43.413')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'375', N'101', N'cbab710134c13e622ecd94d8bdf544ec42a2ae06526d23e4f5de00cf3f98a690', N'0', N'2017-02-20 12:04:32.750')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'109', N'101', N'cbfc8f26eb12f87b067560d174e0e02fa1f19611216784b207c15405770e8f3e', N'0', N'2016-12-28 10:24:14.143')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'291', N'101', N'cc9153226b872b3dd420b9403a185a82e1da0f2d2d28ba17b78fe5d1b11f5301', N'0', N'2017-02-08 15:31:50.243')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'273', N'101', N'ccb61cb5d74282e04b608326b8f1e95ab3395c98ae6987ca70a651753fa880e9', N'0', N'2017-02-06 17:10:46.597')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'462', N'101', N'cd246970a8ddb718b78234ba7a6530c5d3167f5470a7883ea64495288a9d2281', N'0', N'2017-03-03 15:18:20.480')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'647', N'101', N'ce0e1bfb45b5277c91c8f29c3fa16d2bf1f4b1c19492ab1b0b73bbfc7d53ac72', N'0', N'2017-03-23 15:58:48.130')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'574', N'101', N'ce8a5048f02bf255d419480f084906b51aa55704d96c3f11ef0f6760a0d70f1f', N'0', N'2017-03-18 10:42:13.617')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'672', N'101', N'cf1772a118756d2af552058180bdd5364c1dc1863f83f8b50e65c2e286c079fa', N'0', N'2017-03-28 10:26:00.900')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'497', N'101', N'd105ccd7ba73c767a5847069399e377c43eac010e1dca5ed82d6e6d30200d74a', N'0', N'2017-03-08 15:02:45.787')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'126', N'101', N'd108bf0ac76e73cdf2252adf28b03ba1d2e035c7705e238fa33cc1c4c639e85b', N'0', N'2016-12-28 23:28:07.157')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'277', N'101', N'd11abe931b6fa765ad32dae64839b9cfb3395c98ae6987ca70a651753fa880e9', N'0', N'2017-02-07 15:13:05.650')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'172', N'101', N'd1a6cc295a594fd184ad52090a31e4f93f305ae458361b19d282a74b742eeaef', N'0', N'2017-01-11 15:46:55.707')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'490', N'101', N'd309b5d85dfeb6203495b7778ee4a25e248abbbd33477b1e54bc659dcf3b2dde', N'0', N'2017-03-07 16:16:10.333')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'359', N'101', N'd346f4b9e2d4908e7e43a903aaa1b032fa197e85cae2211bea6dbca9ccf2769e', N'0', N'2017-02-16 20:39:02.027')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'70', N'101', N'd443a01d0551213e610bd5a1662d42e57a2828448650b120b32b5c5d0929b575', N'0', N'2016-12-26 19:25:05.867')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'475', N'101', N'd4aa266c7130e1f98bc36a812ec258bcec98d559ea22e9f523c71471f367f893', N'0', N'2017-03-06 10:32:09.640')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'493', N'101', N'd4c8d9afb01d5ac9a00c7a8afc190a09bd0db2373571d56759c8431fb4b1da4e', N'0', N'2017-03-08 09:42:14.183')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'578', N'101', N'd4f84a11655bc212375aedcd5d2202b4a2ddeed26fbf832708b338ff7fb64394', N'0', N'2017-03-18 14:12:30.443')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'285', N'101', N'd655c6a878f66c47c1b4530facc998dee315ff9db71a8fe999d3f3f70ef02640', N'0', N'2017-02-08 11:24:49.270')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'519', N'101', N'd6e96257aceb6e135072ede752b42b85d424a53c8e2bfcbaebc8f746a119a3d7', N'0', N'2017-03-14 10:06:12.670')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'341', N'101', N'd73fdc290dcecc08270d3eb91c69f91c93aa0e79c5cbfbddb17733e8795d5a40', N'0', N'2017-02-15 10:55:59.813')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'325', N'101', N'd78916452d8bbdc07e7db570b1cda5c4f4b72282c0a4b27f04a955874a40f768', N'0', N'2017-02-12 15:43:20.460')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'515', N'101', N'd80ac2a30623eeb2f276711691135b2fe314b7ed925b432df13d650469e8daf5', N'0', N'2017-03-10 15:28:50.910')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'149', N'101', N'd884422f0ce6b7214df0268fcdcfef4380aef9329159d85f95c049a4db8546b4', N'0', N'2017-01-05 14:17:07.730')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'448', N'101', N'd9301e38a980a3be17ef83fc7b1ba129bd8efed78fa49fe063b2fc8a8d85a869', N'0', N'2017-03-01 11:09:51.780')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'357', N'101', N'd9634cd79898ea3ed553a150ff47506122e0ae92e2ccea357167229248a4e529', N'0', N'2017-02-16 17:03:11.147')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'169', N'101', N'd9f391f6d5dc3606f9ae3177ed6ad4d1a3cf91cdf52b0def500dff0207d11dd2', N'0', N'2017-01-10 15:17:24.097')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'158', N'101', N'da585f77ac350b97f04475afe8fcb24873b95b07e2b57c89dfedd8d1a4a94466', N'0', N'2017-01-06 16:26:42.823')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'258', N'101', N'dab6ebf7c0ec86fd06c30e7eed4819c96ba6dbc2dee0e7acd91f0fc4674589e3', N'0', N'2017-02-04 14:55:12.410')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'319', N'101', N'dabcfcf33cae7f77f1f75cee254946b9d237df8a8e9a33cf1c31ca193e396a86', N'0', N'2017-02-10 15:48:34.780')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'185', N'101', N'db028b651ce9dbe881f1453d8edd819e757685749893bdf04128c9bef6a46ef1', N'0', N'2017-01-15 11:41:08.107')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'590', N'101', N'dc384066e1f1373b085c7958ac52ea03621807944f68fa3088688ccfe2d323a7', N'0', N'2017-03-20 18:08:31.527')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'308', N'100', N'de32f4a32d7a2fa2008ee81d9d2dbb6ded84964f3016dda748ac8260bd6fdb4b', N'0', N'2017-02-09 16:11:43.960')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'61', N'101', N'de32f4a32d7a2fa2008ee81d9d2dbb6ded84964f3016dda748ac8260bd6fdb4b', N'0', N'2016-12-23 18:07:28.987')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'482', N'101', N'df52b1d6f1fea2e459df75569f7b8f35675b2fbde2a327ec1222d8245ea1a712', N'0', N'2017-03-07 09:59:33.497')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'347', N'101', N'e0996829ce46c0fe81a053bbbf925afaa523c0bcf7ac1958a51f3e2f656ed9bb', N'0', N'2017-02-15 16:33:38.110')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'544', N'101', N'e0de25075427e0b178adac4c131f84872c6257c1c6a5e557b59c50f6417e307d', N'0', N'2017-03-15 15:58:05.807')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'530', N'101', N'e0e895d3a0ced9cf19b1733c0a812285971d711e59ec0e09836fc11894f7fdaa', N'0', N'2017-03-14 19:40:03.957')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'517', N'101', N'e1a8f040eadd042e4c2b0e7f7297c8a3956957899ea7f835f5a58c78be5c2dbe', N'0', N'2017-03-11 12:00:35.857')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'240', N'101', N'e1f26ed54200e6edbaf96bddb433dc1240a80d646d640b25e09cea442cc6ded4', N'0', N'2017-01-23 12:18:41.020')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'327', N'101', N'e1fa3c97e2202c8d7944710eaf2a4f2120d458a1402717b4a1f7a889bee90661', N'0', N'2017-02-13 09:51:30.727')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'506', N'101', N'e2a0df1fdac7b705475179089d051d005a825816a3840e49e1d22af85a89bf4d', N'0', N'2017-03-09 18:04:50.390')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'173', N'101', N'e32a68e8c385d65b6e3b009ae3b99bfc7572a47a6efe60a3314024606e979650', N'0', N'2017-01-11 16:31:10.420')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'272', N'101', N'e3fb5382f18580faf1949dad53bb714db3395c98ae6987ca70a651753fa880e9', N'0', N'2017-02-06 17:10:18.670')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'268', N'101', N'e3fed5407ce7c87110a227fba50b1217e36293c03087d369265c6991817e1ff4', N'0', N'2017-02-06 15:38:53.527')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'239', N'101', N'e42559291d78dd9d23511c0e6be4ab0ba65e39566f302a66bbc490a10103c090', N'0', N'2017-01-23 10:21:23.173')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'645', N'101', N'e46ee92b32721f6beae97a06483d0736c827d75c1afc782509103bdc0ab43358', N'0', N'2017-03-23 15:23:29.300')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'573', N'101', N'e4847b31dc7584cd0355acc2bf9aa14836f2e3e454f7ed2cebc43bfe30d8d46b', N'0', N'2017-03-17 19:25:28.600')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'267', N'101', N'e4bc403656c55441dd92edc3d10e651436511c638c01b4333e4256ec9e7734e1', N'0', N'2017-02-06 15:17:25.050')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'414', N'101', N'e519efa5d73f0e59212818251f83ea0f9f9d4b54cc49eccdb527d63c7203cac5', N'0', N'2017-02-23 13:36:07.047')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'45', N'101', N'e57477e3beea5ead3dbb541cea70601f0fae116c6ad948cbfe81e7060870566d', N'0', N'2016-12-22 11:24:17.583')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'620', N'101', N'e6215e380d0cfeeaf0b56be7274cb038fe64595b7972a2831d1a4a54fdf3b0f0', N'0', N'2017-03-22 10:16:49.630')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'196', N'101', N'e75da70d77581a14f509e011deea3067586c8e20c714c6463553736b856c2635', N'0', N'2017-01-17 14:43:50.663')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'280', N'101', N'e75fb68cdb5f5e9de7ac9b9a374dd90ac1a0d4499d5c1c1fc12163d9b8d94e3d', N'0', N'2017-02-07 19:01:37.743')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'42', N'101', N'e8549977d1ff125755b7b1e3d3e43312a79abee6318a183b934691766639e2e2', N'0', N'2016-12-21 21:27:17.370')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'132', N'101', N'e9d0d5987d582897a4170630b10ed019d365dca7b3d99058c46bda2550bb3824', N'0', N'2016-12-30 12:28:19.947')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'338', N'101', N'ea27c2b414e49ffd012bd0a01c72a2208407ce975b11b073e3c952eaae7eabda', N'0', N'2017-02-14 12:09:58.387')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'263', N'101', N'eac07fa516c7eb8cee783d9db7ae5cc7b422ee5a611814dacf98f950009dfc5c', N'0', N'2017-02-06 14:56:05.103')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'140', N'100', N'eb45c7e1c84a4973b25e80593c00d1c153759984a9a6cec31a4cb26658fb1e66', N'0', N'2016-12-31 16:42:16.897')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'74', N'101', N'eb45c7e1c84a4973b25e80593c00d1c153759984a9a6cec31a4cb26658fb1e66', N'0', N'2016-12-26 22:17:41.590')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'435', N'101', N'eb95a0ff2a57232bbedce29f40eee49eac05f65edd8547cacf94660be25c404c', N'0', N'2017-02-27 10:24:47.590')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'370', N'101', N'ebff00b192172e0ff8a4882a59673eea47de65cc028d419e5f5860aec49aeb20', N'0', N'2017-02-18 19:33:06.127')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'339', N'101', N'ed74ff664437c270da86198c491ecb4b6c8ce48d03e37b82b43990e6712fd2f5', N'0', N'2017-02-14 12:56:03.427')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'613', N'101', N'eddf3efe219233105c1fa5b44a99c8c800cffd93e3d62c03e67db4c13c451012', N'0', N'2017-03-21 18:05:27.700')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'560', N'101', N'ee008e00106b4e99bd4a5737a5d84a8bc21b445ce8363971e8fec7920f8b83bd', N'0', N'2017-03-17 10:15:47.310')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'379', N'101', N'eea687f1a2898ec5fc7991bc8fbd1755a9477ba8915f9ef793c0d3e87a3ecb1a', N'0', N'2017-02-20 16:45:22.740')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'622', N'101', N'efa8955c34e857034086b4039d4386d131640af526752171c646dab42f033ebe', N'0', N'2017-03-22 11:59:29.900')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'84', N'101', N'f02ec41249b9a244feabe2932a0b4d03df6b91e19e2e3f066acaf56a851dc80d', N'0', N'2016-12-27 10:11:53.450')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'634', N'101', N'f1680f2653917cdcbf4c831e92c3de1dbfc388f86b85f84b045a7d1c1263b34d', N'0', N'2017-03-22 18:05:25.030')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'595', N'101', N'f2260761c80b82e716060ef7a522a627e50fc23bd34de0c17fee5bcba6b52f04', N'0', N'2017-03-21 10:54:03.893')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'175', N'101', N'f25f87c96ea264449a7bf00be0644aa73fc4c2d0ef28798ed10c87a2246f00f3', N'0', N'2017-01-12 15:35:38.720')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'194', N'101', N'f414e6da448cc2d6a7032f46dc14424fa1a1bcf62dd163ad51b405865b519790', N'0', N'2017-01-17 12:37:06.397')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'409', N'101', N'f52e00187653f8f70a30e641c362983725afb021c9401475a002e9c685b58375', N'0', N'2017-02-23 11:09:15.240')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'498', N'101', N'f5a5484b2074c0e943a06afc7071b53e19ec8aed7113da767528b5d435d754fe', N'0', N'2017-03-08 16:36:07.090')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'241', N'101', N'f5b91ff376c6d148960fb0bf405c46cee9ff1f368971ce0efbb00eaf8c53ab43', N'0', N'2017-01-23 12:20:07.580')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'537', N'101', N'f62788167d3a5932b179310e33b22aa8df7b3c7821cf28fdba957132a673ccfb', N'0', N'2017-03-15 11:57:15.663')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'218', N'101', N'f6bbf966b22a17efc4ae7984622e73216529c59845ef753a9ea22fb489296aad', N'0', N'2017-01-18 14:47:19.393')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'142', N'101', N'f6d16498010a89fd16209a096b98391652583d09060e5f3a26d06a1fc9d50b10', N'0', N'2017-01-03 09:22:48.467')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'154', N'101', N'f741ee9ced0d2e3159e44502eaa724688574b5d83086c094e5873c3a17a321f1', N'0', N'2017-01-05 16:58:15.950')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'122', N'101', N'f7b39a96e8bbeb602dea4c97ceb02658231b7e075e8bd9c6fafb63880d6327c4', N'0', N'2016-12-28 17:40:19.867')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'602', N'101', N'f7e0d2ffa90d186c7829e66db77f020e2f83473402d197873cd6fe62a01ce4fa', N'0', N'2017-03-21 15:11:09.097')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'494', N'101', N'f89f86dbf307083cc7005f2540209d4242721732f919c5a8755621f88035bf45', N'0', N'2017-03-08 11:17:36.057')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'454', N'101', N'f94e4aba6931425ff4aa55af71eb7e2394c6c16d2d8c6a27e494702a08da2ce8', N'0', N'2017-03-01 13:11:15.807')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'491', N'101', N'f97dd9e697cb9fb2b4d8791cf1b0268b8792580263f05194fdfefb48ef0bcfe1', N'0', N'2017-03-07 16:51:55.410')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'627', N'101', N'f9973fa2ac2ddcd6c924f99e87f005ef5813ed5893025d95576823e35c0cdceb', N'0', N'2017-03-22 13:37:06.800')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'386', N'101', N'fa413ee906f470a9c6a749f39d478c2b8022e006b1018950ce5d5b99509898ef', N'0', N'2017-02-20 18:44:44.857')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'48', N'100', N'fa9b5398607a059c97a6135106a82a550cc9fddaa41a108b597ecbf7f974a022', N'0', N'2016-12-22 12:15:29.310')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'39', N'101', N'fa9b5398607a059c97a6135106a82a550cc9fddaa41a108b597ecbf7f974a022', N'0', N'2016-12-21 19:30:24.743')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'555', N'101', N'fa9fbc62b1a6274017dc40d3a185613cb75fe29c2b0b529388e580ae718830d7', N'0', N'2017-03-17 08:35:46.980')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'608', N'101', N'faee32021cb588fd35d79399def8474777dd1829857c1877c903248c3582a73f', N'0', N'2017-03-21 16:24:29.897')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'242', N'101', N'faf9bfd34de91d9be3597ec48c32ea1a9e20a633243d2c4fa62319a903aad4e9', N'0', N'2017-01-23 15:03:54.797')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'117', N'101', N'fb21bd4775dfe47b3ec87688a9add93433c8e553bf8d9df0d24a19715dd39435', N'0', N'2016-12-28 15:32:37.960')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'657', N'101', N'fb7ead017fafe978f5b3dc9c9b8a4da7c2f922d2b9b972fddeab7b1e081564ef', N'0', N'2017-03-24 16:06:45.740')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'424', N'101', N'fbbdd8eb72a2f4776cc93da8d8fc8f2c77f14eff10a5e6c27138270ac14acfe1', N'0', N'2017-02-23 18:18:26.960')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'223', N'101', N'fbcd7df0e5c925132de4ab956d72e8636176e6897314f1bdda3dbca66a763f4b', N'0', N'2017-01-19 09:54:12.110')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'449', N'101', N'fbf25a096d6e099fd32885d2cae9de636102797836c222ba7ce05dad5ccdc1a0', N'0', N'2017-03-01 11:13:23.727')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'135', N'101', N'fce8e3b85f993bf3f26033e6de088e450623880b389e4619ffa2f0a335cade9a', N'0', N'2016-12-30 16:30:13.300')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'568', N'101', N'fcecf476d933ff476d394fd0d36249e107c31ab4ea666393d12799510805112b', N'0', N'2017-03-17 15:10:53.147')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'407', N'101', N'fcfdb02c18070ee432a7bca1d4af34c7589cebe51a1c3d46eed60ce56a55e045', N'0', N'2017-02-23 11:02:39.877')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'312', N'101', N'fd8be42b7cbd04cf4c359255e15397ae4904faf9d03a648d1b4ef360725f0a98', N'0', N'2017-02-09 19:06:09.000')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'331', N'101', N'fdcb786664a136b58853981a5bd59b23b7e8c7ac1fda9eb5311db85e0dfeb70e', N'0', N'2017-02-13 15:32:51.303')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'203', N'101', N'fe486dc9d3aaca697abebe24cd1b51abe8b4d77ec344f8d7cbf8a8a0a4c96ed9', N'0', N'2017-01-17 16:27:50.997')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'260', N'101', N'fe7b9a544c221ae449979665987bf26aee7b00894406e8a400eb89eea2a0321f', N'0', N'2017-02-06 10:58:56.823')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'644', N'101', N'feba3705a19647ea0571cb67d2e4aacfdac8e021ae7f4aa6f5e6daa3f9cda32d', N'0', N'2017-03-23 13:52:51.283')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'133', N'101', N'ff830f736d1d562c23ebceac7bf8041e6ead9ea3245b3c9bbfc4ec21666fc27f', N'0', N'2016-12-30 14:02:50.023')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'388', N'101', N'ffbd8f322ae74374e950c70d4f6ac168fd9de7e4ac452a7e54758506b6952e7e', N'0', N'2017-02-20 19:12:34.890')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'575', N'101', N'ffc5daf86edf88a239c63048f81837b08181a92b7ff227c71f5c22b57a5d22c6', N'0', N'2017-03-18 14:11:00.060')
GO
GO
INSERT INTO [dbo].[user_account] ([user_id], [platform_type], [platform_user_id], [user_account_status], [reg_date]) VALUES (N'367', N'102', N'ya29.Glv2A45rzPOeXSnErrJ_QNlqv5IVOfFCRy4nbMd_pmvrdBVxk81fGe2-d89XYuxKDmsHsJvNpLYyQh7P6wojrldvJ6TW7qRFOXCMLQCJNTr98KbjHojiL1DWC1s', N'0', N'2017-02-18 17:32:06.843')
GO
GO
SET IDENTITY_INSERT [dbo].[user_account] OFF
GO

-- ----------------------------
-- Table structure for user_account_restrict
-- ----------------------------
DROP TABLE [dbo].[user_account_restrict]
GO
CREATE TABLE [dbo].[user_account_restrict] (
[user_id] bigint NOT NULL ,
[login_restrict_enddate] datetime NOT NULL ,
[login_restrict_reg_date] datetime NOT NULL ,
[chat_restrict_endate] datetime NOT NULL ,
[chat_restrict_reg_date] datetime NOT NULL 
)


GO

-- ----------------------------
-- Records of user_account_restrict
-- ----------------------------

-- ----------------------------
-- Table structure for user_billing_authkey
-- ----------------------------
DROP TABLE [dbo].[user_billing_authkey]
GO
CREATE TABLE [dbo].[user_billing_authkey] (
[billing_authkey] nvarchar(256) NOT NULL ,
[user_id] bigint NOT NULL ,
[platform_type] int NOT NULL ,
[product_id] nvarchar(256) NOT NULL ,
[payload_info] nvarchar(256) NOT NULL ,
[regdate] datetime NOT NULL 
)


GO

-- ----------------------------
-- Records of user_billing_authkey
-- ----------------------------

-- ----------------------------
-- Table structure for user_billing_list
-- ----------------------------
DROP TABLE [dbo].[user_billing_list]
GO
CREATE TABLE [dbo].[user_billing_list] (
[billind_idx] bigint NOT NULL IDENTITY(1,1) ,
[user_id] bigint NOT NULL ,
[game_service_id] bigint NOT NULL ,
[product_id] nvarchar(256) NOT NULL ,
[price_value] int NOT NULL ,
[price_tier] int NOT NULL ,
[billing_authkey] nvarchar(256) NOT NULL ,
[billing_token] nvarchar(2048) NOT NULL ,
[billing_platform_type] int NOT NULL ,
[billing_status] tinyint NOT NULL ,
[regdate] datetime NOT NULL 
)


GO

-- ----------------------------
-- Records of user_billing_list
-- ----------------------------
SET IDENTITY_INSERT [dbo].[user_billing_list] ON
GO
SET IDENTITY_INSERT [dbo].[user_billing_list] OFF
GO

-- ----------------------------
-- Table structure for user_guest_auth_id
-- ----------------------------
DROP TABLE [dbo].[user_guest_auth_id]
GO
CREATE TABLE [dbo].[user_guest_auth_id] (
[auth_md5_id] nvarchar(256) NOT NULL ,
[server_auth_token] nvarchar(256) NOT NULL ,
[client_auth_token] nvarchar(256) NOT NULL ,
[server_auth_md5] char(32) NOT NULL ,
[client_auth_md5] char(32) NOT NULL ,
[reg_date] datetime NOT NULL 
)


GO

-- ----------------------------
-- Records of user_guest_auth_id
-- ----------------------------
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'007acc8fcdcf6322f2648936b4f813e3d65ef74c25bf3b4798fc266112654a4b', N'q31nBJCJFUSRoAGTLjyOwA==', N'bbee88fc733741d8b26b3760a205a640', N'007acc8fcdcf6322f2648936b4f813e3', N'd65ef74c25bf3b4798fc266112654a4b', N'2017-02-10 14:54:13.017')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'0202b10c33f27639d7fe992645e97641692110b47ec014631a496dfcbae8a536', N'0JIR4UkeRUWdQTRfn6L+vw==', N'd8be9de96e804530809b81163bbd0fca', N'0202b10c33f27639d7fe992645e97641', N'692110b47ec014631a496dfcbae8a536', N'2017-01-13 10:41:39.290')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'0280f7108f313729384b057312f76988a4d3fe160ce675f3675b742e4431edc8', N'v1NZMKruoU6QelHcDt7m4w==', N'320c9e6e8bb94864b7be889cad10208c', N'0280f7108f313729384b057312f76988', N'a4d3fe160ce675f3675b742e4431edc8', N'2017-02-20 17:21:23.000')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'038778d98f3f8d8b5aea43103cf676794413f28d7487062a11f9010f32aa996d', N'NV3ko5Rau0Gt/lnFFu4s4g==', N'daadad7e2bb14746add5972f2a3373ed', N'038778d98f3f8d8b5aea43103cf67679', N'4413f28d7487062a11f9010f32aa996d', N'2017-02-09 15:20:36.757')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'04c82356daf35561d47f6659c85031b64a9325cb3cdbc0f1f3ef0f295f771ad9', N'llzffvjtAUW4zaY/AYzG/Q==', N'02209381f327460c9da626e189d9dca3', N'04c82356daf35561d47f6659c85031b6', N'4a9325cb3cdbc0f1f3ef0f295f771ad9', N'2017-01-05 15:02:42.010')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'051bdef3c9cd10be9c7de96acf9c36b6a50ccee39892f575705f09f36dc4cd12', N'JUWCUNMRg0mektD7F0P+1g==', N'781521b2f897492c82d5da63687b02e0', N'051bdef3c9cd10be9c7de96acf9c36b6', N'a50ccee39892f575705f09f36dc4cd12', N'2017-03-20 11:09:17.530')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'052a20ba4680c5fa8d0ba7e46e53f53235e450b59642ab9f145e1f17267aec95', N'G6O+ipOgVk6i3OQX4SdKdw==', N'978132b8e9574e5b8b20f1cbaf2ab474', N'052a20ba4680c5fa8d0ba7e46e53f532', N'35e450b59642ab9f145e1f17267aec95', N'2016-12-27 10:09:56.840')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'05ab84d5a798383eaa152c64144419a08bc6aca12b706ca4332e88541af8f72b', N'cavz/hED5EuV6jf/vfBz4g==', N'031efd43f6764d04bcc51f3890e7be5f', N'05ab84d5a798383eaa152c64144419a0', N'8bc6aca12b706ca4332e88541af8f72b', N'2017-03-01 12:16:30.893')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'05ae225c0d42cfc5b46c4c6a1a5616f4ee767ed9d5614fecbd0f0b52075a418d', N'yQkfRNBIK0i8sSvkwS2dQw==', N'e6cc647b0a714a81904509a4d8138661', N'05ae225c0d42cfc5b46c4c6a1a5616f4', N'ee767ed9d5614fecbd0f0b52075a418d', N'2017-02-24 15:44:43.413')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'05eec927fd0caa09ea925a1835525d500d834b49e10b124f9897a9c6a834fdc2', N'iaJS1u3gvEWKmn+fKMG0IQ==', N'f3493d8552cd45f88ecba6b027455c08', N'05eec927fd0caa09ea925a1835525d50', N'0d834b49e10b124f9897a9c6a834fdc2', N'2017-03-14 14:39:34.153')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'064153e6c4fe7e67ba9ff88b4c6019b21e425282677741ded90ed52b8044afcd', N'qmanLOZAn069FcvBNbGO7Q==', N'251844ed1be34f53a6f8bd8199a41f75', N'064153e6c4fe7e67ba9ff88b4c6019b2', N'1e425282677741ded90ed52b8044afcd', N'2017-03-03 18:39:23.777')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'0677c7bd2c272a2bfbeb6798b778ac18319719295bee01d505b5bd990b5cff0a', N'G8Z6Hw9sOkisgzNWcUNrsA==', N'e01a84d3f45c43f9976ad0e14626a55a', N'0677c7bd2c272a2bfbeb6798b778ac18', N'319719295bee01d505b5bd990b5cff0a', N'2017-02-23 16:02:27.453')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'06aa484db20200b19429ca88eb1482ca28fd73ffe372bc0b61a6516ee043925f', N'hPtbWPUvwU+pJ97dsQbJog==', N'c1f1f06a1926489c897c69b866840ee9', N'06aa484db20200b19429ca88eb1482ca', N'28fd73ffe372bc0b61a6516ee043925f', N'2017-03-18 14:12:26.757')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'07409b6ce038ac69f3867fc45ae018e0ea79608f3c7503d3e61bd504482ff9ef', N'PCGyTJMJo0OWxp2tB8WQyg==', N'375fbe380adf419cb96ba0c70e66abc7', N'07409b6ce038ac69f3867fc45ae018e0', N'ea79608f3c7503d3e61bd504482ff9ef', N'2017-02-27 19:48:07.550')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'075c65a79760a7fad3385345885505489fed8f4f99941820417dd825b43b7d3f', N'6jEYhr7mjEmJaGWi7kz+pw==', N'6b16158e01d4406aa7ea1394ee341b67', N'075c65a79760a7fad338534588550548', N'9fed8f4f99941820417dd825b43b7d3f', N'2017-03-22 13:37:54.300')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'07a97f34fca6dbc19c3f553ee7e132d31a4ee5f67966b9e020382a755636c58f', N'jsQdCq5Vl0yNIN7UVHRKDg==', N'57d09e1c17f144c8918fe7b7e66ec1ed', N'07a97f34fca6dbc19c3f553ee7e132d3', N'1a4ee5f67966b9e020382a755636c58f', N'2017-01-05 16:10:00.950')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'0852f71eddc08dd1b7ecb11ad639ddf8da0b9caebf35824a70ce586de472451b', N'H6WNOwzFZ0aM6WwXBGF9Mw==', N'4413dbdd36954642ae2a4ee07ff528d8', N'0852f71eddc08dd1b7ecb11ad639ddf8', N'da0b9caebf35824a70ce586de472451b', N'2016-12-27 10:11:48.560')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'089e713d8b7ad3e1495dd0b83893d95375b5531b2a4658442fbdb157fc6764a6', N'R0h8nM02qECVdGl19dpo2g==', N'589fc9e7b3de42b79f0d10c7b57b87fc', N'089e713d8b7ad3e1495dd0b83893d953', N'75b5531b2a4658442fbdb157fc6764a6', N'2017-03-07 10:03:40.473')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'08cb3bcb233048954a0cc3c8622ac58d3b193e3506215490e0a5e524bcb89eab', N'U3xE4Qaz20ikqXR5+dYnMg==', N'089bb19786f94af3858f7f22fd5257cd', N'08cb3bcb233048954a0cc3c8622ac58d', N'3b193e3506215490e0a5e524bcb89eab', N'2016-12-28 18:53:50.320')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'08f31fb2f60be07fe9fdf1bcb0341ce2215459709b099671a506b3e4d300e68e', N'yH8AHVrnfEyTSRPZ7a2sGQ==', N'6c311086a42148d3bef121941a5a1cf7', N'08f31fb2f60be07fe9fdf1bcb0341ce2', N'215459709b099671a506b3e4d300e68e', N'2017-02-23 14:14:36.797')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'092696b4e4ab00a3905f8837b9bfe7b34ff969e23777f88afff29d8d4f059a0f', N'9k3qQGOmEkq/ViekLsRwmQ==', N'79b836ae186940af803d3017931ebaad', N'092696b4e4ab00a3905f8837b9bfe7b3', N'4ff969e23777f88afff29d8d4f059a0f', N'2017-03-22 18:44:57.243')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'09a31e936ec3a4d7d11693f035dc9c1af6c293ec8a503104022d278074e4c31e', N'1dpHZIKVBkGfVyp5hwHU8Q==', N'9d3c0845fb864f6a9738a1b0e90e439f', N'09a31e936ec3a4d7d11693f035dc9c1a', N'f6c293ec8a503104022d278074e4c31e', N'2017-03-22 15:30:23.250')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'09dc170569310b60750d1b251a2ced0e409227f6ef01f4c798470d18fff4d1d1', N'++2jVSUHu0WNVOTBb9bI/g==', N'72e511fc0d5c49d2a3299d7c026b5e86', N'09dc170569310b60750d1b251a2ced0e', N'409227f6ef01f4c798470d18fff4d1d1', N'2017-03-07 14:02:20.423')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'0a8f6fe3f081b37ff97b1cce55fde74fecdad62aa3e5cd4c3ebeadb7e930039a', N'xeSoZ6ggsUeBXXK+aJMr/w==', N'f749c58511224288aab3b7c2933f6743', N'0a8f6fe3f081b37ff97b1cce55fde74f', N'ecdad62aa3e5cd4c3ebeadb7e930039a', N'2017-01-17 17:30:24.073')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'0adc08340d6fbdc559d3de52ed8618ea9b5933957cbfaa2a036b65d13f76298d', N'V2Az4KXZ/Emm/sciQElY5g==', N'7c682b6af1da4fc088354ec5ccde4b49', N'0adc08340d6fbdc559d3de52ed8618ea', N'9b5933957cbfaa2a036b65d13f76298d', N'2017-02-08 11:09:11.340')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'0b394fb6b60c75b0747ca19ad63405c2ba7339d2920c62d51b74e205f7f1d3c2', N'ytHwjXHrfEuRMQnfdPS5ig==', N'e9bfa6733548432e9efbe33b3c4bff10', N'0b394fb6b60c75b0747ca19ad63405c2', N'ba7339d2920c62d51b74e205f7f1d3c2', N'2017-03-20 16:18:16.477')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'0c267db75d06053984f50e5b98f004e1a01961c9f567d0e819d37eb55e2fc08f', N'4IksyLhmPU2uailjfBYOiQ==', N'f4f27324badb4aa6980a3f7411503eb6', N'0c267db75d06053984f50e5b98f004e1', N'a01961c9f567d0e819d37eb55e2fc08f', N'2017-03-17 11:43:30.450')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'0cf0b351b9b21337fdf478d3212d6740433354eff19ee9c2926271302479f44a', N'aN3PuXxWM0qR+t73a6Xn9g==', N'3857d05689164fe1a1796e9bfa43ec91', N'0cf0b351b9b21337fdf478d3212d6740', N'433354eff19ee9c2926271302479f44a', N'2016-12-30 14:03:16.350')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'0d33383112e1bd007fca26809fdd8b5b8b297cdb0291f3a926cc13d0e7fa35dc', N'mNyoaNB6y0agGbO+kqCNhg==', N'447678de53b14ed6921d459ac6184a2d', N'0d33383112e1bd007fca26809fdd8b5b', N'8b297cdb0291f3a926cc13d0e7fa35dc', N'2017-03-20 20:57:12.653')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'0d39bdfe02e728a9713c37beeeb226750c106eaf44dfa347319a32952b0d0e10', N'QleDoSgROU2hsz2Lwutzyg==', N'e62ab5fe378a4383b71601d700e8cbf5', N'0d39bdfe02e728a9713c37beeeb22675', N'0c106eaf44dfa347319a32952b0d0e10', N'2016-12-26 13:36:40.220')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'0f6551e726c393629fabd39eb5b83607db9ab4feecae85edd604a2da2ae7c9d4', N'1aqnjahmN0uoufGiMz5bSw==', N'3d42a3a4f49b44eaaacf4aabcf3814d3', N'0f6551e726c393629fabd39eb5b83607', N'db9ab4feecae85edd604a2da2ae7c9d4', N'2016-12-22 14:06:17.080')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'110ada72683b50417a821747751f703ec6d4e76f837d28b51a524976953585db', N'XXux2hu/DUi8VzR6gELovw==', N'9f6a814d51244cd0a45c5cbfea1878bd', N'110ada72683b50417a821747751f703e', N'c6d4e76f837d28b51a524976953585db', N'2017-02-06 15:10:01.537')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'116eaf9e6788d812cc51afe7e3f6d66e3cd2936254e538d7ed99ee2125781747', N'yd5GcHuhn0KimGuJofsupA==', N'39698356f3684981b39467777346b82f', N'116eaf9e6788d812cc51afe7e3f6d66e', N'3cd2936254e538d7ed99ee2125781747', N'2017-03-17 10:09:48.123')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'11d3310fd2405030abe5ba9e9f8bd985a25a4dacb3ecf8dcb9a945e9fb83c383', N'0ME87aJT8kuEewcjnXQuDw==', N'bbebc638181d4761b2db10fa4b5f2ebe', N'11d3310fd2405030abe5ba9e9f8bd985', N'a25a4dacb3ecf8dcb9a945e9fb83c383', N'2017-01-10 10:22:23.867')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'1220bae66e44aec509bba6b82968d9d8e8616368ed6b3f33f0040b984e359860', N'vsCMq+Bm9kagS9C2B+SVAg==', N'6ed456c200b14f56b3a3355b6c946bf2', N'1220bae66e44aec509bba6b82968d9d8', N'e8616368ed6b3f33f0040b984e359860', N'2017-03-07 14:02:12.157')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'126f74ebf43fa1d938608308518330e2b6128bbf1aade33db1065585e00c46d1', N'yrUnLW47JkSHWdA2cr7F4A==', N'd5fd215ab6184bfab539eb4ee0ccf081', N'126f74ebf43fa1d938608308518330e2', N'b6128bbf1aade33db1065585e00c46d1', N'2017-02-09 15:26:34.910')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'1281b325b1b4209af8070e2303054478a051f9184ff38bb24abf71799e9e7a8e', N'LYQmc/AOhkWE7wQKXksFZg==', N'3028e2d8c7974d0ea03591a598b600f7', N'1281b325b1b4209af8070e2303054478', N'a051f9184ff38bb24abf71799e9e7a8e', N'2017-02-09 12:31:07.890')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'1295b665aea39866c3adfd52fe1c9ffdb63f43599cb2381f30e94b2895630b84', N'V16oYKKhNUmcEAknNkeHVw==', N'eb51efde54da45199510354d1e16ad62', N'1295b665aea39866c3adfd52fe1c9ffd', N'b63f43599cb2381f30e94b2895630b84', N'2017-02-08 17:14:52.520')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'14af8b773f7bc7dc1975f0909c192ba44c3a927306e024e74cf0a71646bc6ed1', N'msaHCx7a8kC6ynVOzJFK0g==', N'344358fd633f47fcaf37619a59924b97', N'14af8b773f7bc7dc1975f0909c192ba4', N'4c3a927306e024e74cf0a71646bc6ed1', N'2017-02-09 10:29:59.347')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'158afa502798ca0e8c3b6227f4deeff9d1e3c70c4d196dc899bf054d54186526', N'nCVkVbxjC0ev1RM8QEZhrQ==', N'49de32c7e48d486c800e5012b128a93f', N'158afa502798ca0e8c3b6227f4deeff9', N'd1e3c70c4d196dc899bf054d54186526', N'2017-02-09 16:30:30.707')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'15ac3de6acfefdeaaf9447f3f21625e37ac7a9949b593550cc2d471867000cb7', N'BuOTYLXsLUOQivEVsU3KsQ==', N'cbec4dad71e747dc86a775a69546bd7d', N'15ac3de6acfefdeaaf9447f3f21625e3', N'7ac7a9949b593550cc2d471867000cb7', N'2017-02-20 18:36:58.363')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'165bbf63689e05c524fd04f44d86caeb5728e76f811b028c2adccebf1e7e4b43', N'Lw07u6PHOk2QLycmiz785Q==', N'd9b8a205ef7a4926966639368c42d6e2', N'165bbf63689e05c524fd04f44d86caeb', N'5728e76f811b028c2adccebf1e7e4b43', N'2017-03-10 20:01:46.703')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'172e918bfb53cbeeaed6f0c7ac12e91342a2ae06526d23e4f5de00cf3f98a690', N'KD0Fqlfoxk+qr8X2AMSGHQ==', N'E95A4BF8-B216-4177-B12A-4CCE0C00401D', N'172e918bfb53cbeeaed6f0c7ac12e913', N'42a2ae06526d23e4f5de00cf3f98a690', N'2017-02-15 15:24:00.053')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'174d9e2d5034657f29faa320172f0d64a069c27d7095e1b5c7d16a67cbba79f5', N'8jwvJRpzTEScr48VfOh0HQ==', N'12fc1f7dc8e94c13a0ce65dba38d4e38', N'174d9e2d5034657f29faa320172f0d64', N'a069c27d7095e1b5c7d16a67cbba79f5', N'2017-03-01 10:38:24.760')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'1802a9acf5225ed5bdaee1fbd7cf14941926de4502932a8c1b26b92e50f2e31b', N'PmcmdGZCWUKyKiLwcRfHlQ==', N'5890fbc1e4404765803e2478eb3fbb31', N'1802a9acf5225ed5bdaee1fbd7cf1494', N'1926de4502932a8c1b26b92e50f2e31b', N'2017-03-21 21:33:53.810')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'1924c43cbbdd4a79b7fc539c0b2a2d8010ccff809fe8b48f9af520bb46e094f1', N'gxXk9Q7B/kOyEnlgssMkRA==', N'5230acd740734a2f8e86aab2c5d1fa27', N'1924c43cbbdd4a79b7fc539c0b2a2d80', N'10ccff809fe8b48f9af520bb46e094f1', N'2017-01-10 11:54:38.540')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'19905b0bb6b6c19fbd6621fdd086236e964bd221dff274c1352e7aaccec69df4', N'vsyvWBJN60yGPpZOThLr6A==', N'4b1f30974cee48d6a32cb298d43e58cd', N'19905b0bb6b6c19fbd6621fdd086236e', N'964bd221dff274c1352e7aaccec69df4', N'2017-03-14 09:52:14.060')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'19a1dd12330c1602d95736d838274fb4e7ee0d511aa1a021e7fa34a814019720', N'HztUQcrjx0mvyoFUEMH9Mg==', N'895a21977aed4208b2512af19a0f074e', N'19a1dd12330c1602d95736d838274fb4', N'e7ee0d511aa1a021e7fa34a814019720', N'2017-03-24 15:09:03.213')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'19b8c1ccd1a9d32a794220af86c4f921a0626c89e57dfca9fdadad1a61f24c21', N'uDPzbKY6DUOABhbJsy/lxQ==', N'2f3b31a8fe7a496985014dcbff8cab25', N'19b8c1ccd1a9d32a794220af86c4f921', N'a0626c89e57dfca9fdadad1a61f24c21', N'2016-12-23 17:22:58.187')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'1b7e36a20a3228094de48901cf36d8f16451b0364b55e4f6a65bdfeb28aeb081', N'3QWNJ/Rj0k6z648YtJXJwQ==', N'605fc541f5c047e99bf144b3309278e8', N'1b7e36a20a3228094de48901cf36d8f1', N'6451b0364b55e4f6a65bdfeb28aeb081', N'2017-03-20 11:09:05.670')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'1d4a44400b9cdec61e7d3eb6263635e0309b9856d5dd2224187feee3ecd1280a', N'pMvj+WzxlEmmOOhOFZpBog==', N'6bbbbb83d0274af184e7aad6f404bba2', N'1d4a44400b9cdec61e7d3eb6263635e0', N'309b9856d5dd2224187feee3ecd1280a', N'2016-12-27 10:58:52.400')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'1de6b3744049bc393c0d8601a563302b942e3a356b7ce59d690d077b083c827b', N'uOaD1xzUaE+GujRuLJMHHQ==', N'188e973819d84919bda8b8030e90c1fb', N'1de6b3744049bc393c0d8601a563302b', N'942e3a356b7ce59d690d077b083c827b', N'2017-02-10 17:01:09.070')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'1e1644f49d4629c43805d6918a69d36fc64072d129a9d295812525b7445dd339', N'2dAK6pHWa0y7lJV87KtkuA==', N'c6be8a54fc7541d5ba241fcaae638aae', N'1e1644f49d4629c43805d6918a69d36f', N'c64072d129a9d295812525b7445dd339', N'2017-03-21 16:17:09.260')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'1edb7d22bd5b904c1e8c9cff7f10c558859d8f6a8053da776e956a990a2003a8', N'eVcQ/G0g1kWr8zul67pbcg==', N'79ca45a9695044dabb9ec54141796efe', N'1edb7d22bd5b904c1e8c9cff7f10c558', N'859d8f6a8053da776e956a990a2003a8', N'2017-03-22 10:22:04.473')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'1ef928663b0d0b42d353b601c9e09a48448f33c93e1a1b8c5cc7423009b5e115', N'TYBEM6HLOUqcV5nt3bK54Q==', N'd617926c64b248a18b9ff89f0ffc857c', N'1ef928663b0d0b42d353b601c9e09a48', N'448f33c93e1a1b8c5cc7423009b5e115', N'2017-02-10 15:48:32.770')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'1f15c26804007426412ecaa0406fad8007205025a66f5bbd02d1cdcc8e357413', N'HdlautgprU6UoWciizNJNw==', N'040cb07082064c0a9135ece143c872f7', N'1f15c26804007426412ecaa0406fad80', N'07205025a66f5bbd02d1cdcc8e357413', N'2017-02-11 11:04:13.070')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'1f7b051c77464907b715342c581f3b40652f3fa9b8989622fd4f7b79c9c91e7f', N'lgAkvd5/7k6fdvJiBy6/XA==', N'7ff0b17e75d942719065c3cd92039a5a', N'1f7b051c77464907b715342c581f3b40', N'652f3fa9b8989622fd4f7b79c9c91e7f', N'2017-02-08 09:56:36.990')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'1f83382ddfc5f9dc276135c5c7c59e41f80be033bae6848c511fc1bc86155e82', N'Cxtk1CqZZEaSD7hJhj5TSA==', N'daea0bfdb1a84fa2b009fc9f7fda6183', N'1f83382ddfc5f9dc276135c5c7c59e41', N'f80be033bae6848c511fc1bc86155e82', N'2017-02-24 16:42:30.830')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'20b283aac9c05ebf3f32826fe9863e69592b10b8a1401461e6aa9686c7939e38', N'0/zK+sRy606l0aO5WC4fww==', N'bec5193849bf4f5bb18edbadf2aba7ad', N'20b283aac9c05ebf3f32826fe9863e69', N'592b10b8a1401461e6aa9686c7939e38', N'2017-03-24 00:26:31.217')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'212b381d86fe6c7c0b9f20e2fbf039574dc2d8f9f7f947d5a5efa0b64e764bcf', N'CVbsyTDvQUy28+jboDm7uw==', N'31db8ed181c546779dece5e8adf81026', N'212b381d86fe6c7c0b9f20e2fbf03957', N'4dc2d8f9f7f947d5a5efa0b64e764bcf', N'2017-03-10 13:01:40.117')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'2148b579f1404c3d1661446ae7afcae32e9d2c453d965cdc64b6d0de787fd290', N'atjMg5Uy20W8LiDIPCZ+zQ==', N'60f7206705a54c0badf4e6e8ee1ed6a2', N'2148b579f1404c3d1661446ae7afcae3', N'2e9d2c453d965cdc64b6d0de787fd290', N'2017-03-07 14:50:35.437')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'21c5dd37865b7101cf3558e709b37e5335bc4c11fec0d15c86cb151836d8eba8', N'oLkpJm4010Oe8sDMXpAOYw==', N'e6c3f001f2144090ab68d3204d12f382', N'21c5dd37865b7101cf3558e709b37e53', N'35bc4c11fec0d15c86cb151836d8eba8', N'2017-01-06 17:05:47.290')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'21ebb91f8caf90ade9612251aef3be16ec44e9c5b6e418c975dcc467e98e6667', N'Ko+P2mLiQ0S4kjV82oyhRA==', N'59018af1ace54a0a8aab92d36c729bad', N'21ebb91f8caf90ade9612251aef3be16', N'ec44e9c5b6e418c975dcc467e98e6667', N'2016-12-27 14:32:21.367')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'2224cdf9a34b43ed6c24fc0ab220f0ab9c7a2902458f35e09a6f8c3cf7e5436c', N'F+S4xaNZ5E2NtV39p793mQ==', N'5c6710fbb54f4382a00483b0a6e1d847', N'2224cdf9a34b43ed6c24fc0ab220f0ab', N'9c7a2902458f35e09a6f8c3cf7e5436c', N'2017-01-17 17:43:55.047')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'2283c0ccd22f1eefd68fc1e70ae808365cbc976d7208eb317215a1f81d95e80f', N'6Hze2bVwMkyXdrenXvsYNg==', N'4a6e798a2cb541a5b328e5ab8fb924f7', N'2283c0ccd22f1eefd68fc1e70ae80836', N'5cbc976d7208eb317215a1f81d95e80f', N'2017-03-14 10:22:21.053')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'22bde1bc12369d56a21a4545b470ac83eeb069397d75b8011ffd941630593c3c', N'qs9+efYK0Em28b/n4C8IYg==', N'02487eeb1049405894bac9605cf34c21', N'22bde1bc12369d56a21a4545b470ac83', N'eeb069397d75b8011ffd941630593c3c', N'2017-02-23 13:55:28.823')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'22e6cf83bd3e3160d90d7a06e03f6d8526cee90890c69d2441083c03b8985600', N'9HC54jCFc0eOgCxjZcTLWA==', N'523bb1364f9b4fcb8b18d97ea68da9f4', N'22e6cf83bd3e3160d90d7a06e03f6d85', N'26cee90890c69d2441083c03b8985600', N'2017-02-20 12:07:00.447')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'23acb729cf6bf62d45fd4bf0f5183041f3b3882e3dbc7a99c1d0a6616d560fc4', N'r93qdDB3RUOm3rvSVz1awA==', N'99f4dec1a30d4e6492cdfe8ccdb56a8f', N'23acb729cf6bf62d45fd4bf0f5183041', N'f3b3882e3dbc7a99c1d0a6616d560fc4', N'2017-01-06 16:05:36.640')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'2496a3b4a1c5ed114697e056c780913df285fd4952a90f12a66dc5979d6efb89', N'NbJ7Mz5sZUW3HEUSmv8Czw==', N'27be2da634c44819ad41e027ea469ab1', N'2496a3b4a1c5ed114697e056c780913d', N'f285fd4952a90f12a66dc5979d6efb89', N'2016-12-28 10:49:04.923')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'24bb5f2e18d921a26170dc5e394370cb21322b68ac27d45fc2680666d56e774d', N'9FyarBL/kkmnZqNfoln/Jw==', N'c816eaa25b544309a7ef88c8b068fe6c', N'24bb5f2e18d921a26170dc5e394370cb', N'21322b68ac27d45fc2680666d56e774d', N'2017-03-08 16:48:59.723')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'24cdb9442da0c48785f72bd6b6ab92f334556ed6b5f52a72021583b901adf78f', N'bYItZ4hZakqdA9FzLRvv4Q==', N'12a5abc14ef043b59c365547fe2f7470', N'24cdb9442da0c48785f72bd6b6ab92f3', N'34556ed6b5f52a72021583b901adf78f', N'2017-01-03 13:35:25.077')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'258926e15cf34b973ab93e271be2fbef4989ada848069684dddae7b46c7af939', N'H4zYUirH/0CAqA283yWcPA==', N'6db43a94d82c4e398a75669c1b4da0e0', N'258926e15cf34b973ab93e271be2fbef', N'4989ada848069684dddae7b46c7af939', N'2016-12-23 17:57:09.033')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'259cf3d5e1ff6be3d79bd9c914f673ded750e3319f47f50669aa8c12d1bb7b02', N'SINgvkx3GUOr4Wvm7mRwiQ==', N'b91e786b28e548ceac37ca395a228af7', N'259cf3d5e1ff6be3d79bd9c914f673de', N'd750e3319f47f50669aa8c12d1bb7b02', N'2017-03-27 13:29:06.187')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'259d04a8e557fe498e5d62769281b670870a6c16ed49537ba5024d282387c6f5', N'1ph4qnVGB0ewLE+Qpw2KTg==', N'1e68c1b55a5543d9874cb755eb3f6e0e', N'259d04a8e557fe498e5d62769281b670', N'870a6c16ed49537ba5024d282387c6f5', N'2017-03-23 15:32:51.643')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'25ad5ab2bac9706c4226ab9d5cd24d4c1ed81d74f8d1dd85d6125d6aa7aeed86', N'7MGGgbc+kkaUYttqJbrd3w==', N'953f0236c4ff4fbeac70d8ab0cd094bd', N'25ad5ab2bac9706c4226ab9d5cd24d4c', N'1ed81d74f8d1dd85d6125d6aa7aeed86', N'2017-03-06 16:55:33.820')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'25d649e876396bb94b544515aeb07074f1fd87caf584f0e49ca68d68e6c13647', N'Q1h9hgASZkeufmg1rtFuwA==', N'069a701142f44745a842a483fd453173', N'25d649e876396bb94b544515aeb07074', N'f1fd87caf584f0e49ca68d68e6c13647', N'2017-01-12 19:35:45.197')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'25d7804a5a0d795e84a1198fbd27b10c605976e01a979024241de620c0dd9c98', N'JqU2JT7wYUCkRr5BDirLHA==', N'e72e72d33fb2410c90a80605520c718c', N'25d7804a5a0d795e84a1198fbd27b10c', N'605976e01a979024241de620c0dd9c98', N'2017-03-18 14:12:27.950')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'26da300849b097a69ca4c7df35e197c70885a9496018fc7e93b64ab89c2341d8', N'iVxK7NIh1Uec5WWh0qp2YQ==', N'a95caf56f26e493b8c55fafc84fc6007', N'26da300849b097a69ca4c7df35e197c7', N'0885a9496018fc7e93b64ab89c2341d8', N'2017-03-17 10:45:26.700')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'26df9bfbdd4982eb833f08714609d597a8fbdc31f37a22bc9ad5f86b5b878187', N'RNJlv4dR0Uef+hfaKBDQ0A==', N'f4625ab07ba749c091ba0387a0bb1702', N'26df9bfbdd4982eb833f08714609d597', N'a8fbdc31f37a22bc9ad5f86b5b878187', N'2017-03-16 17:52:47.980')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'279ef717eb750c6b9f373c7316f4a88e5425f405d47b5f95369deece70ad7515', N'r0ippTw7sU2zrYOcLRi+cw==', N'ffe1bc5774264d52811d41737e3597b9', N'279ef717eb750c6b9f373c7316f4a88e', N'5425f405d47b5f95369deece70ad7515', N'2017-02-23 11:15:16.350')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'28ed5b5bcbc4d810facb6df6ce5f3e52475e6eb2fd80a2af45693719239775d6', N'A2scTWTJRku5sYM9/hnNhg==', N'5c88cf2fddb94cdbbc9541d9f2d54898', N'28ed5b5bcbc4d810facb6df6ce5f3e52', N'475e6eb2fd80a2af45693719239775d6', N'2017-03-21 14:08:27.937')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'293eb238c1bad6b6a89455309c67cd6ed52063390f8677fba6dd35f3b7834fe3', N'TMOtQVm5XEmQX/0ukjxbdw==', N'fd8eb7f7c7664a1ebdb05e47727cda6f', N'293eb238c1bad6b6a89455309c67cd6e', N'd52063390f8677fba6dd35f3b7834fe3', N'2017-03-20 11:26:19.297')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'294279b7b3bc02217e3c2d6d73a810a1d9e4b22863095e4ee83ebc154c78182b', N'Fpu1SkXvokOI0RL+yjWVgg==', N'0b1690e4f37b4425bb63e539d80cd119', N'294279b7b3bc02217e3c2d6d73a810a1', N'd9e4b22863095e4ee83ebc154c78182b', N'2017-03-03 15:19:45.180')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'298fb900e1d248c952312f0999c94aa8edc8f79080c8125dc38ccb54030797c6', N'Mzc4L6UmnUSWx32uj9Rpyw==', N'f1e6061560694a1b9380abad913b1f88', N'298fb900e1d248c952312f0999c94aa8', N'edc8f79080c8125dc38ccb54030797c6', N'2017-02-16 11:51:11.113')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'2a6481f38ec9aa95a9900cfc6f864d2fb837a9bafe3d860ba662a6e805a76f72', N'BJO0lSy7PEyMeqA2PE7Nrg==', N'a8131702ff5e42c691dc0fab19e3bd68', N'2a6481f38ec9aa95a9900cfc6f864d2f', N'b837a9bafe3d860ba662a6e805a76f72', N'2017-03-01 14:19:52.820')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'2a7a029056a22bdb3f5fa8e8b7957f18bf8f952286c6d8ec8a010f600070e98b', N'ydJlcC+fCkS38jQQXiOc3Q==', N'c62b09b262cc49e898a06ae0fa044996', N'2a7a029056a22bdb3f5fa8e8b7957f18', N'bf8f952286c6d8ec8a010f600070e98b', N'2017-01-09 18:23:11.007')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'2aa02e64f51b9ffaae949a663fbfa2199967ecd0865f1a9bffaf2570927be1fb', N'JCF69429EUuQthvtrY1hHQ==', N'45137d41de9841319eb8ff95f5024bac', N'2aa02e64f51b9ffaae949a663fbfa219', N'9967ecd0865f1a9bffaf2570927be1fb', N'2017-03-23 21:37:32.653')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'2b748d67d35c5869dd8395aeb9b5d9cbe6b328b6d7e0061a01901a92c4896637', N'+xdtnSG2BUiaALxbUPqKkg==', N'eb1068aaec184cc79f1e9160a305f121', N'2b748d67d35c5869dd8395aeb9b5d9cb', N'e6b328b6d7e0061a01901a92c4896637', N'2017-01-17 11:33:45.423')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'2b79b0bc0f129e35645391b547f3d50252f335eaf8742b2b229569733af0f4de', N'0gHOh9tukEGI8BnojePqZg==', N'84c0d346815f428b880b4c1dcd8a9fd6', N'2b79b0bc0f129e35645391b547f3d502', N'52f335eaf8742b2b229569733af0f4de', N'2017-03-28 21:32:56.610')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'2b898e946313cf74923c246f322ce2f2715a5ee3d4f03ce80b94660fdd0f12a8', N'1JWvYMUFJ0O2TqZt2slmaA==', N'9183daa5489741efb8b8d12f6e8c5e4b', N'2b898e946313cf74923c246f322ce2f2', N'715a5ee3d4f03ce80b94660fdd0f12a8', N'2017-02-14 10:34:03.090')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'2d4c6a2a63b080c17aeb2a81e3858a0092635ba63883415be79af00e5ca0de04', N'6jQty0OVgk+jSu7irD7SqA==', N'e87474d1ed8e48f1b9dc998bc5b0166e', N'2d4c6a2a63b080c17aeb2a81e3858a00', N'92635ba63883415be79af00e5ca0de04', N'2017-02-22 15:00:26.960')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'2d4cb9690db6fdd46aede8345e97d41e9af4725d78cac7ff6c5db5b93f7523e9', N'2mpz8nFkQ0G9RR0Q5b12IQ==', N'4812e3ecffb848e2b0d8b5ed47516b01', N'2d4cb9690db6fdd46aede8345e97d41e', N'9af4725d78cac7ff6c5db5b93f7523e9', N'2017-03-16 10:00:29.307')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'2e16b09cda3b9877441202929423cc898a56d860d2752d6f8c29bc10110fa343', N'nY8yrB892ECnv/7K4LBCaw==', N'201099513187426e84c6213b4169ba31', N'2e16b09cda3b9877441202929423cc89', N'8a56d860d2752d6f8c29bc10110fa343', N'2017-01-17 11:52:29.827')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'2e72d85339b1b6dda5ed29039181eebb3fe8da45b9cbc82c912b5f362e5f723d', N'UNLfDagVkUadQMBqhY9DdA==', N'56748cfe21934e8f824287d63cbb0506', N'2e72d85339b1b6dda5ed29039181eebb', N'3fe8da45b9cbc82c912b5f362e5f723d', N'2017-03-14 10:08:33.247')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'2efe5170caf0244c66ffc46bae1fe5ae301b7f088d4e76756af6f84fc377c9b1', N'LaRRp3+fwUqaieSlCZy+Jw==', N'e02bdfb741c24f28acd3a98bb8021cae', N'2efe5170caf0244c66ffc46bae1fe5ae', N'301b7f088d4e76756af6f84fc377c9b1', N'2016-12-28 10:53:37.650')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'2fce38164d78cebbae0db089887126df08ade00ae624eea7c5adda708da11d32', N'm/uO1DgjbEuR0aiG3BlVFw==', N'6fd3124c3aee463a8e51e8cb697370e2', N'2fce38164d78cebbae0db089887126df', N'08ade00ae624eea7c5adda708da11d32', N'2017-02-20 19:11:17.720')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'3075a4efe76ae45e5612d626faed977671e4caa4e90c287227679020c31ec157', N'4IB41ao+4EuIx3XM7l4FlQ==', N'3aa67758f4d249c080c10fd6e5b1563c', N'3075a4efe76ae45e5612d626faed9776', N'71e4caa4e90c287227679020c31ec157', N'2017-01-17 15:14:48.783')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'30f8108f0f5e4d09f2bc1423d172091a143c45f6a6f9e676961cd3340cfa0a6e', N'fnZXDvD4z0GH7sYh+X67VQ==', N'797b61d8ae9d4020bf9cca66b86640d8', N'30f8108f0f5e4d09f2bc1423d172091a', N'143c45f6a6f9e676961cd3340cfa0a6e', N'2017-03-21 15:51:10.113')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'31414ae3f60a69680eca020204bd377cd2b1b2f22632854049e60d0a7e511176', N'cjSuKOK/+EytnLpl4A2jEg==', N'f103994257884ae5bb5ba5592c13d355', N'31414ae3f60a69680eca020204bd377c', N'd2b1b2f22632854049e60d0a7e511176', N'2017-01-23 15:54:49.997')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'32b2456e904e5e58cc64ed94ada4515a053b599b73bf14d9912e27a31b1ffb4b', N'NicUbcjHBUuSNEc/ZeKQJQ==', N'05add49c69ff4c259bd65bf4d852b92e', N'32b2456e904e5e58cc64ed94ada4515a', N'053b599b73bf14d9912e27a31b1ffb4b', N'2016-12-26 14:25:07.343')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'33587184c3821a7463c52b29d9d076fe651a304753f02a8ad575cdb0a1f29168', N'zlufynF760Kl/qqYATBZxQ==', N'92e12d23f1564187a7d0cad81e3a4016', N'33587184c3821a7463c52b29d9d076fe', N'651a304753f02a8ad575cdb0a1f29168', N'2016-12-21 20:51:39.287')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'338dda501c03080989f9fb44dd4c485f35b6f391cdb22cb8fb18754a0c2fdfab', N'p1F6DKhi802i5tjCwV4TXw==', N'543a796cdf18424eaf06a8508d6186e0', N'338dda501c03080989f9fb44dd4c485f', N'35b6f391cdb22cb8fb18754a0c2fdfab', N'2016-12-30 11:59:31.530')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'33fd9d6eaa0a35cd6a9f3ffe3b87d9c6760cff411a62cc7d8cc3f2505bd23fc6', N'YAhNFaldoEOiXCeM4cWKWg==', N'e98b1d6fe96647c3bcef154f705c3a49', N'33fd9d6eaa0a35cd6a9f3ffe3b87d9c6', N'760cff411a62cc7d8cc3f2505bd23fc6', N'2017-03-17 08:39:50.387')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'342eb42fa6ce7014379b31c78b9e0df71cf95d09ae12adeac0eb1775a013d888', N'eDJJTfv/FUWv0U/EsptMQw==', N'A4B9B24E-A654-4052-AC72-6607A6BF60C2', N'342eb42fa6ce7014379b31c78b9e0df7', N'1cf95d09ae12adeac0eb1775a013d888', N'2017-02-28 16:43:35.077')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'3560a6d26809d3a3a3fa050de6c6975d7963440b0d29855bac093e50366253ba', N'oHj9UyQhpUeZ2ICKMwav6A==', N'ee60f8e0e3bf439aa983f7411c7454ae', N'3560a6d26809d3a3a3fa050de6c6975d', N'7963440b0d29855bac093e50366253ba', N'2017-02-23 16:08:54.523')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'358b1b106045b10312f64e30078594fcfcec9600c8ee7e5a8ae023d19a43c364', N'75BMwEl7qk2OYww7Qob7Mw==', N'7bb86f885a3744d8aa93106ef9995789', N'358b1b106045b10312f64e30078594fc', N'fcec9600c8ee7e5a8ae023d19a43c364', N'2017-01-17 18:18:53.303')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'3593a31ee8c8cbf9282e02620cb14280a48833cb35cc4d1da53efce64db94c83', N'ST1ZGzh6E0yq2CLKXx793A==', N'16b0fba79967455785a32a69f4ed81d4', N'3593a31ee8c8cbf9282e02620cb14280', N'a48833cb35cc4d1da53efce64db94c83', N'2017-01-05 16:14:33.330')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'35e2c3b30f8150d3086f50aa1496f5fd25f328e9de90a63b37bf6457891b5465', N'8TFpQmDO5E+AH26rRWLzmw==', N'eb04915e2aee46fa959a2598202150c5', N'35e2c3b30f8150d3086f50aa1496f5fd', N'25f328e9de90a63b37bf6457891b5465', N'2017-03-17 09:51:17.483')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'36df9fcde3256324cd17ec6207da53c2de01a3282b606ba58b630089bb7da6c8', N'vbxJNjykp0yT6M8ghS5p3Q==', N'ac66acea63d24019b06feb065655e5d9', N'36df9fcde3256324cd17ec6207da53c2', N'de01a3282b606ba58b630089bb7da6c8', N'2017-03-24 20:10:03.350')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'36edda42d12195c5e3a85c3cb7d07f2124af7f8d5468c77383ca7f6978da6687', N'5oxV02sWN0S12g+la4d3sA==', N'e672ab4401364581a6c97675e6e9da69', N'36edda42d12195c5e3a85c3cb7d07f21', N'24af7f8d5468c77383ca7f6978da6687', N'2017-02-01 19:37:18.573')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'36f2d0e8a922bf587dcf9b9fa6a5b379a970f6ff65c4f5ac406e5c767b612246', N'K7aIilkpxEmIyWFrN30a2w==', N'13cb00f381464b00bb56982090355d8a', N'36f2d0e8a922bf587dcf9b9fa6a5b379', N'a970f6ff65c4f5ac406e5c767b612246', N'2017-03-22 19:03:50.540')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'37fb82c35cbf540d392cf1f1280403b42459ada23753160c5f43c58323ada66e', N'mKQt3KyPzUSrS27OrSiwEw==', N'a566b1fb00374c79a36ca63df305ffc7', N'37fb82c35cbf540d392cf1f1280403b4', N'2459ada23753160c5f43c58323ada66e', N'2017-03-21 16:52:26.633')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'3922f02e59460032f3254e4b0fddea3dd11ba2415da03bf22b5543f78f46c3e4', N'JAkrdesluUOQ1I4RQssTgg==', N'ecd7943eed7f42458345be1e9c408a86', N'3922f02e59460032f3254e4b0fddea3d', N'd11ba2415da03bf22b5543f78f46c3e4', N'2017-03-18 17:27:04.740')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'39552489f3373e236cb603b2084f481a8eda28ca7f8c971a29efe2aa2dc08620', N'rJxOcZB8tk+AIwC+PsNj5A==', N'9ac9dad0962a4fcd96e0834f93a12053', N'39552489f3373e236cb603b2084f481a', N'8eda28ca7f8c971a29efe2aa2dc08620', N'2017-03-22 12:03:06.150')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'39940e6d0603d9da151507cc044b55a6b3395c98ae6987ca70a651753fa880e9', N'nnIUPHQQekaSKAq6oLqJjw==', N'6cf6ceb287ce4ee9bd992c5b75148efa', N'39940e6d0603d9da151507cc044b55a6', N'b3395c98ae6987ca70a651753fa880e9', N'2017-02-06 17:12:17.020')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'3ae953a43836397b1b337965071be397831330d124caad5a55975c306ff8de5e', N'74ArYGtGt0Stxn4tWgcBuA==', N'f70fb841b3a245d298c3d2f9c79ede6c', N'3ae953a43836397b1b337965071be397', N'831330d124caad5a55975c306ff8de5e', N'2016-12-27 10:10:21.927')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'3b6c120de05e0d86edb7620c5d280f24bdef108cdf41982f2a6ee0dec9ff3c90', N'mG9DNtSrRkW7j9KaFq7q+Q==', N'121bcee4fc374594bed7e0c315ffca82', N'3b6c120de05e0d86edb7620c5d280f24', N'bdef108cdf41982f2a6ee0dec9ff3c90', N'2017-03-16 17:40:08.683')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'3c0117971f1d039e1c8931270ea6b3d2a539933ad64a84f3ac46f988d07ff8cc', N'WyVxJHTIJ0ufWm/gtN1DTA==', N'faeb5a9fc9624e01b0873b53814899fe', N'3c0117971f1d039e1c8931270ea6b3d2', N'a539933ad64a84f3ac46f988d07ff8cc', N'2017-02-09 10:18:08.237')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'3cdbb8961c937d97f0cecdbca4e5b1bf5b63328358ec6a145b66ed34c134982a', N'ov0EyOsNMEq4Sj5GgtVTRg==', N'27c7d77a9a7d443a9380bb3b9715ff3e', N'3cdbb8961c937d97f0cecdbca4e5b1bf', N'5b63328358ec6a145b66ed34c134982a', N'2017-02-18 15:42:15.020')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'3dae798e65596d6a16c3e493d7447fee5edb1ec1d6052a9974ac0f712ef83cfc', N'JB7AnZUAZ0SAp9vW4n7PVQ==', N'61ce42359c3b44b78364b84c0eca38f5', N'3dae798e65596d6a16c3e493d7447fee', N'5edb1ec1d6052a9974ac0f712ef83cfc', N'2016-12-21 19:03:55.397')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'3dc0d7cf447857bbbef1a1d349c30f48c33ba80ff5ad5225d6dfd9dd29fdc406', N'PIW99iTULEmWvLP33wuXaQ==', N'f77a048fd4aa442ebba1f14b7dba03b8', N'3dc0d7cf447857bbbef1a1d349c30f48', N'c33ba80ff5ad5225d6dfd9dd29fdc406', N'2017-03-22 20:00:53.710')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'3ddd7126a525acd4cdbee559abe9f003c80199dc106f2ff979d3cdeb28e47096', N'h8p33gmRHEK9MrtGXJtSrA==', N'5346b50d9aa34e52882876d89457eacb', N'3ddd7126a525acd4cdbee559abe9f003', N'c80199dc106f2ff979d3cdeb28e47096', N'2017-02-06 11:02:14.727')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'3e6c0becac326c82dabd8fd929cbbbdb527d99918ce635489c2b678f922fb7f5', N'74Lq41scwUebNXJRST82zQ==', N'8367854a1ce849da94995ce63c2b1f51', N'3e6c0becac326c82dabd8fd929cbbbdb', N'527d99918ce635489c2b678f922fb7f5', N'2017-02-22 12:47:20.443')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'3ed21d13edce99485cdee35aae2f12e130d3ab25f09601bc6bb16d7646364d75', N'wk6jFweEaUurV2B9yYbOgQ==', N'f450c08d34e444179da79091fea3cf8b', N'3ed21d13edce99485cdee35aae2f12e1', N'30d3ab25f09601bc6bb16d7646364d75', N'2017-03-01 11:08:09.357')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'3f347bf9c84321a4f703b7935c695f07b3395c98ae6987ca70a651753fa880e9', N'QJJwzOnmW0KD9y+j0d8lsw==', N'6cf6ceb287ce4ee9bd992c5b75148efa', N'3f347bf9c84321a4f703b7935c695f07', N'b3395c98ae6987ca70a651753fa880e9', N'2017-02-06 16:06:04.653')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'3f91106662464c755b80084fec5999787a9fa5f18d75ad3b8f530fd440497891', N'BrgFYYSVGEenr0AOhW53LQ==', N'd25b69ee8b9c42cd99aa5bce049238ff', N'3f91106662464c755b80084fec599978', N'7a9fa5f18d75ad3b8f530fd440497891', N'2017-02-13 15:32:23.597')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'3fb9149c8f8eb247ce5e163ea821a09db7d5a9c331baef6d099e6dcded3e21ea', N'06v0Ue0KBk+jt4e1ScwjkQ==', N'de39def7826948b195f60838617a77b6', N'3fb9149c8f8eb247ce5e163ea821a09d', N'b7d5a9c331baef6d099e6dcded3e21ea', N'2017-03-15 11:17:59.400')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'3ff67eaca2868a6afe8694c09e9b869dfb42c244b30ada82476e2da5de44c314', N'lPT+nZtIaU6oMcGnYIpsDg==', N'8a857b695d284ac480afe58f2e7b4bdc', N'3ff67eaca2868a6afe8694c09e9b869d', N'fb42c244b30ada82476e2da5de44c314', N'2017-01-06 12:10:37.250')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'4027f7c61c20f96fa2fa40622853050b7af00b99e9ac8a91cb68ca8d7aa4b6c8', N'XHGqXrV6Y0inhCgr/gDgDQ==', N'df9a4958446a4bd6ad5182bb482027f0', N'4027f7c61c20f96fa2fa40622853050b', N'7af00b99e9ac8a91cb68ca8d7aa4b6c8', N'2016-12-28 14:50:57.613')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'4059647fc024dddb756f74b44c30c887012edac72d20770fbc071f9979b77703', N'sfSZJ8OG60eZ3cwB/7XSVg==', N'f208ceae20e64419b0c76278eb14cf8b', N'4059647fc024dddb756f74b44c30c887', N'012edac72d20770fbc071f9979b77703', N'2017-03-09 15:21:47.387')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'4079fc15865cabca80cb870fdcc700d242a2ae06526d23e4f5de00cf3f98a690', N'RebrkS0uIk+xznnrKTLp2g==', N'E95A4BF8-B216-4177-B12A-4CCE0C00401D', N'4079fc15865cabca80cb870fdcc700d2', N'42a2ae06526d23e4f5de00cf3f98a690', N'2017-02-21 17:00:25.273')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'40889c0e6b79bb092df0013f2891729285eccbcd5a335a700eca7539e19ad2ad', N'JI/nFCnjA0Wblu7QqkNDqw==', N'85559310dc3a42d59c212d8eb4621a3e', N'40889c0e6b79bb092df0013f28917292', N'85eccbcd5a335a700eca7539e19ad2ad', N'2017-01-17 20:02:41.150')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'4093b1d48187e18cd2c6097775a40afcda05a0934b4f2643df890d8e2556fe5a', N'ikGNUMkW9keFeDXlM75Y+Q==', N'9a28da862cd94828b7379f519b9c96ec', N'4093b1d48187e18cd2c6097775a40afc', N'da05a0934b4f2643df890d8e2556fe5a', N'2017-02-23 14:24:31.397')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'40b2171322da7cc2d330a8f8f25d52151cf95d09ae12adeac0eb1775a013d888', N'PcH5qLLqt066/OhRNi7OCA==', N'A4B9B24E-A654-4052-AC72-6607A6BF60C2', N'40b2171322da7cc2d330a8f8f25d5215', N'1cf95d09ae12adeac0eb1775a013d888', N'2017-02-28 16:38:03.617')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'40cc684dfcd14a669bbc2c5e3f79a798a41fdd560091b5829fde9db93b486fb6', N'0VVHbL4iv0a08WtGj8K2Fg==', N'be6bf01a54bb4b509d5d69055e391587', N'40cc684dfcd14a669bbc2c5e3f79a798', N'a41fdd560091b5829fde9db93b486fb6', N'2017-03-22 14:01:47.820')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'40eae2140da6cecda79121f5c03991fac5dbe265653c1de66362b4ed8689dcae', N'6LCp/l7UHkexpHs2iwb+1g==', N'7712f62e76ca4529a38953ff51909110', N'40eae2140da6cecda79121f5c03991fa', N'c5dbe265653c1de66362b4ed8689dcae', N'2017-01-10 13:23:41.063')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'410ebbe5e91aa75a9135dacb1dcf231fe48cb870455b4e68cce0d77dc5af3820', N'hPVNALxjQkKGJBA/LHDPFA==', N'55ef14efd1004ab693761cbf471def15', N'410ebbe5e91aa75a9135dacb1dcf231f', N'e48cb870455b4e68cce0d77dc5af3820', N'2017-02-21 17:02:06.960')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'4140c24ffc6afb3eaf09b4e8811581b0c3568791a7c23ab7e45f57dfbd083b88', N'bsaj1twuEE6/U2anVgBlsw==', N'fab5a8dabd0f4235873660b5c5b63fb1', N'4140c24ffc6afb3eaf09b4e8811581b0', N'c3568791a7c23ab7e45f57dfbd083b88', N'2017-03-28 13:35:22.593')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'414cd49896d15903f8aef0139411cf941cf95d09ae12adeac0eb1775a013d888', N'IGBa3WEVL0i/mXggf8s8YA==', N'A4B9B24E-A654-4052-AC72-6607A6BF60C2', N'414cd49896d15903f8aef0139411cf94', N'1cf95d09ae12adeac0eb1775a013d888', N'2017-02-15 17:50:12.093')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'4278912de134c554c1ba2fb801bcebf3f47d16195196ead9100265eefcf66262', N'YsF7hgMA0keBJo+sWcjFRw==', N'aeef35de3f2f4e02a12f2e0d4aecf546', N'4278912de134c554c1ba2fb801bcebf3', N'f47d16195196ead9100265eefcf66262', N'2017-03-22 16:44:06.543')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'42912c75c683e315c5d68f2bf09915febb5443210ef72bf5092049d4e344f7c0', N'pQZr7QRx/USzu4p3GyxWqw==', N'0c7347b35f164152902ab1f7d11edbef', N'42912c75c683e315c5d68f2bf09915fe', N'bb5443210ef72bf5092049d4e344f7c0', N'2017-01-07 12:30:02.240')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'44438141f495f136aef2b5350efccec186a9f45988bd3bc57fc05657c9171715', N'9p84S+1tf0iWyFHMSRcBjQ==', N'7007d11de9ca4ab7a555d3637b957bb1', N'44438141f495f136aef2b5350efccec1', N'86a9f45988bd3bc57fc05657c9171715', N'2017-01-10 16:36:25.973')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'444f97ed0c92a43c77f04b5ecb493292641dbd55bad102331e0812fe4d2f6656', N'17qcWOXUPU26KU5E9V3lUA==', N'aedafea4a2cb4741b175040e669bd214', N'444f97ed0c92a43c77f04b5ecb493292', N'641dbd55bad102331e0812fe4d2f6656', N'2017-03-07 17:07:03.327')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'446ebc9cfcc248be13767190384d08499b04153aba6a42f4ff11de21b3774cda', N'TQSWDqdzPUuNgwUi5G+aAg==', N'3555cc50b4eb44ec87822be0476d71f6', N'446ebc9cfcc248be13767190384d0849', N'9b04153aba6a42f4ff11de21b3774cda', N'2017-03-06 15:43:54.993')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'45191e6a2e4e89a3b54e7c7a27e2f9ed0df6c0ecc2856cceadbd1c2cd8986fcb', N'GEUpHmz0FEmRTqEabLgvwA==', N'd7603be2ce37452890f2d05667cdbf38', N'45191e6a2e4e89a3b54e7c7a27e2f9ed', N'0df6c0ecc2856cceadbd1c2cd8986fcb', N'2017-03-22 09:44:32.923')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'45ca5a23dcac374eb5f39b313458c9f8f0c2fe37fe1049f8fc4067367b933bd2', N'IA9+n9TUL02IY/YZ4cM8Hw==', N'49d3a182b0654e88964dc8b9d78f6723', N'45ca5a23dcac374eb5f39b313458c9f8', N'f0c2fe37fe1049f8fc4067367b933bd2', N'2017-03-17 10:01:02.750')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'463f37ec77ef3b8b44d0e383ba8f6860d263bbc4d0f837ab84aadc327e4ed688', N'vLX1TbOwJ0CCGNVyvt4oSg==', N'25c397ca9c1447f2922cdff120fbb6b3', N'463f37ec77ef3b8b44d0e383ba8f6860', N'd263bbc4d0f837ab84aadc327e4ed688', N'2017-01-17 16:57:22.900')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'465b2e032643d4f112ba68939189cecfa24ae1126d8e54ce8806443018ee8dd1', N'gYmB5f183ka7RHPWvz08Ww==', N'f25e80a906484f968a0a8fa4e18a466d', N'465b2e032643d4f112ba68939189cecf', N'a24ae1126d8e54ce8806443018ee8dd1', N'2017-03-15 13:00:34.603')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'466009cc6361d48ec1280a78fa9023dfe4bd20d9c90e82adf4d27fe4818b0c82', N'YodZodi2NEuw8gBzG63/og==', N'2ab9aa2668514b2f91b8eea9fcb02dd6', N'466009cc6361d48ec1280a78fa9023df', N'e4bd20d9c90e82adf4d27fe4818b0c82', N'2017-01-09 18:18:46.490')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'478dc4ec90e16e59ae6d308b68d9d3b634290c0cfc0de69c1d0147d851a0ccbf', N'DAYQztGIoUGHfDAREhSn1A==', N'ec1745a34428417caced32d7a408363f', N'478dc4ec90e16e59ae6d308b68d9d3b6', N'34290c0cfc0de69c1d0147d851a0ccbf', N'2017-03-03 18:04:52.840')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'47a840b91547ee0b6d91e1a92f7841d2df9b846141298b23e6e0265b734f618a', N'BDmAUn73sUS6NjeJFJTSjA==', N'3c3a302beefe48f1a8c18545772387d1', N'47a840b91547ee0b6d91e1a92f7841d2', N'df9b846141298b23e6e0265b734f618a', N'2017-03-15 16:21:06.803')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'485058f1a56aa5b4ea24f9fef5b5c8e93cb89afffe3432d909a093fec35560a9', N'eZQcfI6IYUe+RgxathlofQ==', N'a82162ad5a494ef8b20208ac89f3e3a4', N'485058f1a56aa5b4ea24f9fef5b5c8e9', N'3cb89afffe3432d909a093fec35560a9', N'2017-03-27 12:26:27.363')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'48886eec73e445cc9b3eaf13124ec6c8746b697de4c3a2147526e75863d9664e', N'QcT+UPCvzUCuow0KAe+zzw==', N'd29e1c0e642543af97c1eb8d6f6fcd82', N'48886eec73e445cc9b3eaf13124ec6c8', N'746b697de4c3a2147526e75863d9664e', N'2017-01-04 15:45:48.887')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'495096bb50d7cb6af87c8b3f0ae8f305f7992696055039000ab4fe5fed03c032', N'fPP8cBLd9UKoVvl+oXbfEQ==', N'2be7ede97800409cbf702436ca68be7d', N'495096bb50d7cb6af87c8b3f0ae8f305', N'f7992696055039000ab4fe5fed03c032', N'2017-02-23 12:06:45.487')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'49756c363d045819b1c022384f3b0813bde8bff43b60af5421c18c4944d8ccec', N'EjLr3knv/U6oSFTlbuT1nQ==', N'90aae79e9e6e4c0e8ff0676507ec65d1', N'49756c363d045819b1c022384f3b0813', N'bde8bff43b60af5421c18c4944d8ccec', N'2017-03-28 19:07:56.440')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'4986cedabb61b44181d63e4ea2484d4b2ad6dd22e3d46a50e91db195b4d8ff75', N'Xekbpb94M0Kew+WdievjIQ==', N'fe3a438845d64732830baf20fbe1f7ed', N'4986cedabb61b44181d63e4ea2484d4b', N'2ad6dd22e3d46a50e91db195b4d8ff75', N'2017-02-09 15:23:45.570')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'4aeba78f197202cbc6feb6a307959e91576625bab0705e646cb514f849bae0ba', N'kwCgDQDdj0ObQ/oyaDXThw==', N'41d0b13119434d76947f7967f8889a50', N'4aeba78f197202cbc6feb6a307959e91', N'576625bab0705e646cb514f849bae0ba', N'2017-03-01 11:14:07.867')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'4be2b84bc67a2ce61f8cfdc9f106b3ca22fe3dc70d71b1d57a6fd159fe005464', N'Tpqjp5hJaESH/7znyBkaqg==', N'a4df9cd61fca45c0b5a040bae6de7b7f', N'4be2b84bc67a2ce61f8cfdc9f106b3ca', N'22fe3dc70d71b1d57a6fd159fe005464', N'2017-02-23 18:40:26.390')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'4ce3c08fe6e0f57f0e4a139a051a9268aa28dd42120a7d102256b810a6b484a9', N'fYNZfPnZx02W5AXDyJfTRA==', N'39c4330ff8024d7dbe1afcdc473c6490', N'4ce3c08fe6e0f57f0e4a139a051a9268', N'aa28dd42120a7d102256b810a6b484a9', N'2017-03-03 17:08:33.283')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'4d8ec4f0cb5030e8f4f1b31b2e5a38751cf95d09ae12adeac0eb1775a013d888', N'gmLIeqVed0WGlgdoE6ELLQ==', N'A4B9B24E-A654-4052-AC72-6607A6BF60C2', N'4d8ec4f0cb5030e8f4f1b31b2e5a3875', N'1cf95d09ae12adeac0eb1775a013d888', N'2017-02-17 18:12:47.707')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'4de3f10aafd8db533766eefc80b9f009ac28f9a7c6e6b57548384e53141f80d5', N'+0UZA/9hBU6RTY/DkuDAeg==', N'b8466f8bfd9f4a55ac65590fc585ab5a', N'4de3f10aafd8db533766eefc80b9f009', N'ac28f9a7c6e6b57548384e53141f80d5', N'2017-03-17 18:52:49.400')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'4deaed703aa0d4a8544b3527453176f06871cbbd84ea48ac516a40d7ba03b0d8', N'sNjYgXASLEqcxN2oxRtkAA==', N'56105eee19714e2e87eb276fb59bc0bf', N'4deaed703aa0d4a8544b3527453176f0', N'6871cbbd84ea48ac516a40d7ba03b0d8', N'2017-03-16 13:33:17.727')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'4fb4641416ca45abac782cef7f8e3ca5364d1bdb42074233a54c213887ec8075', N'YbnMC2ibEUqVkxNHVNUozQ==', N'c67b5dc7fc524b428de73f8c7422a772', N'4fb4641416ca45abac782cef7f8e3ca5', N'364d1bdb42074233a54c213887ec8075', N'2017-02-02 16:35:05.927')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'508831b9176f7da1cedfaa237930b3d44275dac0a56966806cb6cf62c09eb3e0', N'dgRORdHiEEmrlmcDMOjL7Q==', N'f0cdc86354174b06a0af81421e5ae9fa', N'508831b9176f7da1cedfaa237930b3d4', N'4275dac0a56966806cb6cf62c09eb3e0', N'2017-03-04 12:26:59.537')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'510ea5b7b4ed93c374b7ba4c1da1b7449bf4d5ec991850a2fa2f75a7d12c4b2c', N'qdtjXp90rE6K218svy0RbQ==', N'7e287eab6c2247059380ab27b52b3e46', N'510ea5b7b4ed93c374b7ba4c1da1b744', N'9bf4d5ec991850a2fa2f75a7d12c4b2c', N'2017-01-04 23:41:56.580')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'51b1e19a7d921c954c3092033caa08473d2cb1717d71c8ea591116165692ef12', N'fBu1IPz71ES9Lfke8ElCZQ==', N'b0eaa5d651f7413ca5e5896cd6d1f732', N'51b1e19a7d921c954c3092033caa0847', N'3d2cb1717d71c8ea591116165692ef12', N'2017-03-15 14:31:12.313')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'52f35a4f818ec52a21cd8b099354a9b21f4b31a4d519d67b48e43abb42c7056f', N'z5y+fGoYi0uIeCR8yl9xqA==', N'45c5c85280fc4ecab458377acd100b9a', N'52f35a4f818ec52a21cd8b099354a9b2', N'1f4b31a4d519d67b48e43abb42c7056f', N'2017-03-23 12:44:43.173')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'53452d8535ec43f494377d2d25321cd889ad70da80c809ab0b1f6fbec89223ea', N'1S6KeSc6U0+CfE+L0xSY1Q==', N'd25cb7f06f50461cba44d6edf29b06d0', N'53452d8535ec43f494377d2d25321cd8', N'89ad70da80c809ab0b1f6fbec89223ea', N'2017-02-20 12:09:40.223')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'541c27c31cdbeba824097eb0028db07882720b4e6534c257b801b7c7b8ae2f24', N'KAg7BcLZfU6n9CbWzIEoFw==', N'acb574d68b41410fa68eaf984a71f45c', N'541c27c31cdbeba824097eb0028db078', N'82720b4e6534c257b801b7c7b8ae2f24', N'2017-01-23 21:19:08.627')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'55840658e347a1207d74b872653f03708e1e66fcbea2b8ef0e9c33b7060b15ce', N'mW03aS1S4kuPzWBpSy1Mwg==', N'2ef4eac80e18497da3d3f70d3c8d16c2', N'55840658e347a1207d74b872653f0370', N'8e1e66fcbea2b8ef0e9c33b7060b15ce', N'2017-01-14 18:56:04.793')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'55c485831310b8789f11a81725418c4a72821e26e68f99895d2bb646b63114db', N'chToYTN0IEiZ/upEaFdiPA==', N'da02ea0ea1474cb5ac22b8bcf5cfb662', N'55c485831310b8789f11a81725418c4a', N'72821e26e68f99895d2bb646b63114db', N'2017-02-26 15:46:04.967')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'5659425d31fd60660dc6ef278bee3b047797dcd179bf3cea8181c3024d256dbe', N'aG54KY/z7U6n5cEZw7OpZQ==', N'5088d398557f4a2e9d48f9fa52db9495', N'5659425d31fd60660dc6ef278bee3b04', N'7797dcd179bf3cea8181c3024d256dbe', N'2017-03-09 16:25:10.150')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'56f54a90bae18eb2597f95ef41baa3978a56d860d2752d6f8c29bc10110fa343', N'lPbTi0VUlEePu4nFHPD+Og==', N'201099513187426e84c6213b4169ba31', N'56f54a90bae18eb2597f95ef41baa397', N'8a56d860d2752d6f8c29bc10110fa343', N'2017-01-17 11:56:51.310')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'57da39b903f7a37058a7377906bafd4838ea622f4781e50b57f24bd861c91cca', N'DKW0h5oR3USnmd9zWLT4mQ==', N'9e37ed6ed15e400e8ea17d7061e17af7', N'57da39b903f7a37058a7377906bafd48', N'38ea622f4781e50b57f24bd861c91cca', N'2017-03-10 11:31:03.690')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'59af00bf2d8f4d47bcec00e4dee7942fc014b0b98341c292bf49a8c31ae6429d', N'WK+cHePP90a/ZPQC41i/eA==', N'5f0a02cc3b1646379b5a3d268f1dd607', N'59af00bf2d8f4d47bcec00e4dee7942f', N'c014b0b98341c292bf49a8c31ae6429d', N'2016-12-22 10:59:59.030')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'5a2df86129a1f3ca9551fd55e41632be27957e969c2144aca8f10c82ab9f38bd', N'y3kCi6X21EahmSC5MGE+LA==', N'5bd8c1c1252341f4aea7830c480ec9e7', N'5a2df86129a1f3ca9551fd55e41632be', N'27957e969c2144aca8f10c82ab9f38bd', N'2017-03-10 11:50:48.947')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'5ab269358b47cbcb7067e012b10dafa39c9d99974da6c939c5dd57903d9fb109', N'SQb/IqvAyEijhoMWSSGsng==', N'ee1fe9d181e546cd8f7b84a741787e59', N'5ab269358b47cbcb7067e012b10dafa3', N'9c9d99974da6c939c5dd57903d9fb109', N'2017-01-18 14:49:40.670')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'5ab5db3f73d4cef42ab1d6c974bcf750e1ff58c8b149b9641f7f25bb5a2adb53', N'vgDtCXl8ekqnke4q2TnErQ==', N'61893cb492414d0ebc76706bd639b2c7', N'5ab5db3f73d4cef42ab1d6c974bcf750', N'e1ff58c8b149b9641f7f25bb5a2adb53', N'2017-03-21 17:46:54.630')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'5ba308d018599c7f5f0e194974481047b55721033dad0b011bd2252faeadd03a', N'rI5qtZld0UqVt2JAezPJag==', N'f34dc11fc861427a8d09b9f3d8f7ec00', N'5ba308d018599c7f5f0e194974481047', N'b55721033dad0b011bd2252faeadd03a', N'2017-02-28 10:23:46.550')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'5bac967660e30184ae3738e757329426c89269329c981b0505f6312864d0be54', N'm9BBIIWJJUOzhCjB2540HQ==', N'f7c418ef7a0541bc8355cd4db7aacefc', N'5bac967660e30184ae3738e757329426', N'c89269329c981b0505f6312864d0be54', N'2017-02-24 18:06:49.577')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'5bcc8871853d5ba80723f2b37203086adebdd4cdf36fdb570c2d9b1d8249a5c7', N'jq5WKDU/l0iQViW1We/NAA==', N'582a38e115fc4cdd8679324e83a8b6f6', N'5bcc8871853d5ba80723f2b37203086a', N'debdd4cdf36fdb570c2d9b1d8249a5c7', N'2017-03-23 19:43:25.180')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'5c254096b2fc2d217f9af367f9b59fc909b82c5639221f1000224a32458a8f95', N'dsrL6xGXf06xIXip9iFkvQ==', N'2406d90b28a6475eabf1baab2008d5cc', N'5c254096b2fc2d217f9af367f9b59fc9', N'09b82c5639221f1000224a32458a8f95', N'2017-03-22 10:03:15.930')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'5c28dbbc44b7b0b4e3395ed12d677f80fbd0e1138030f81d539c8cef6dbc3884', N'GV1/qcmlb0yJ5GrksONPUQ==', N'e5683498818549afb2360325b427fa5d', N'5c28dbbc44b7b0b4e3395ed12d677f80', N'fbd0e1138030f81d539c8cef6dbc3884', N'2016-12-25 23:40:40.340')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'5c761d1de3939ac329cf5c2808c3c2bf236353e14af6367d7c5ed2bd7ca6dd23', N'mI6oMXMiXUa3qOjNyCgL+w==', N'a5cd7d6721924a06bcdc61f038e5e7fd', N'5c761d1de3939ac329cf5c2808c3c2bf', N'236353e14af6367d7c5ed2bd7ca6dd23', N'2017-03-17 14:40:01.843')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'5c847170ff76601ef8ee995f1b2b5f1161a66fdfd3a83a80a75e53fb0ce16301', N'oJojWw9foEu3OY6GvjXnJA==', N'7a46ba3bc22a404d9bf14ee4a83fd7d5', N'5c847170ff76601ef8ee995f1b2b5f11', N'61a66fdfd3a83a80a75e53fb0ce16301', N'2017-02-08 15:03:33.153')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'5d4aad5d335bd2767d39227d7b4e9f6c0c5d16e6bb5c2694eda13bee1b242e1b', N'sjkFvuSL002FC39y8RdiUw==', N'98286252e34147f080338941a2875096', N'5d4aad5d335bd2767d39227d7b4e9f6c', N'0c5d16e6bb5c2694eda13bee1b242e1b', N'2017-03-24 17:56:10.047')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'5e3d60b35ea4ad7fa0a409da655b01d3b183f2acebcb325432ade99b15c8a278', N'o8H1K74l80+V7wAONUWBvg==', N'7c4184380df9418a9ceb7f7476d9df4f', N'5e3d60b35ea4ad7fa0a409da655b01d3', N'b183f2acebcb325432ade99b15c8a278', N'2017-03-15 10:29:54.380')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'5ea0610c27101b87d7352df8a849038300e08d08036c60cdaffa6e6f4816b32f', N'rRe97dJTbka/VDXj1sghmQ==', N'fbe2342bc54641a8a4c3ceb326211022', N'5ea0610c27101b87d7352df8a8490383', N'00e08d08036c60cdaffa6e6f4816b32f', N'2017-01-06 14:04:35.370')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'5ef2b3e1ea008ac3a1545223a070df21b9ad48c31c460f074222c738c0b522fb', N'bfimluNuh0OJ/qzMgj9M4A==', N'53938e5e0d554334968da04a96aff9be', N'5ef2b3e1ea008ac3a1545223a070df21', N'b9ad48c31c460f074222c738c0b522fb', N'2017-03-17 10:24:16.467')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'5f19479aab91629371876c5a51fdf8e15472c078f9ca2d8189315213ed6071a8', N'B/NBNUll6keTDsxbJzxwOA==', N'1018eb4101b944aa9de52bcd63f9db97', N'5f19479aab91629371876c5a51fdf8e1', N'5472c078f9ca2d8189315213ed6071a8', N'2016-12-24 10:15:19.220')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'5f8dd7d264f8e291ee8d4ffb9a3812308518629db7c7e7d8d0dfa580f08a2718', N'GGDjoUEHd0uOdclos4v/2g==', N'f84183c372fa451283485177b476c6e9', N'5f8dd7d264f8e291ee8d4ffb9a381230', N'8518629db7c7e7d8d0dfa580f08a2718', N'2017-03-22 12:03:53.527')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'5fe6b4697a54e1c760d34655760a1509ad015e82547f901a993f63e87f3e0c92', N'TxzUdpG5oEOddK+9EMTJcA==', N'c038c7a15dd84b4c9437eef627b48e0c', N'5fe6b4697a54e1c760d34655760a1509', N'ad015e82547f901a993f63e87f3e0c92', N'2017-03-15 11:15:54.150')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'60351c7d3893d08379f79c0b1743f6a59d174dbbb49f2570d60db6b8a9f4d712', N'ng6MehYcGkGKkbqZx4EYIg==', N'5f2e86f468cb47009ee8f82a0030bfed', N'60351c7d3893d08379f79c0b1743f6a5', N'9d174dbbb49f2570d60db6b8a9f4d712', N'2017-03-06 17:31:34.237')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'6039cd64a07b48a90f182e4e2c7c6cdd2f5aad538f4101604343f586523149dc', N'enn+pl1rXk2G1YZcwtB0kw==', N'6b476c66ef6f4a01a48fe512f1b89ea0', N'6039cd64a07b48a90f182e4e2c7c6cdd', N'2f5aad538f4101604343f586523149dc', N'2017-02-14 10:58:06.043')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'60728990bc9879866f7813feed90945b704ba5a3a998d6023be0359d4e9d138d', N'QZfVX7XvqUSm2V04Kq3kHQ==', N'ed32831ab28f4846ab08cd27d63f6170', N'60728990bc9879866f7813feed90945b', N'704ba5a3a998d6023be0359d4e9d138d', N'2017-02-07 17:22:56.210')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'609ab334c03e2d881023dc9e7f09ca88cb0a2601f97ab204de4ffbc012ca7efe', N'YG77ybxZ/k2uvLAjwaPhiQ==', N'75fde1b379894e9d8793ce43fdaa7414', N'609ab334c03e2d881023dc9e7f09ca88', N'cb0a2601f97ab204de4ffbc012ca7efe', N'2016-12-30 19:03:38.453')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'60a3613afe8a14273f37087ff35582916faada1a36d10ce86d227c00420aa1ab', N'Z5qHfhUM90Gc1lgW1sGWKA==', N'53b332396cd642168939dc655492ca71', N'60a3613afe8a14273f37087ff3558291', N'6faada1a36d10ce86d227c00420aa1ab', N'2017-03-07 15:43:03.860')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'60fd76272a7ed4e8ea142efe78b67cbf4a5607be2e2e8a92305abcd2b7538d4f', N'QDRZnDJ4pUmHb60A4/xz4Q==', N'a2c8fcc7cc7e454997ee86e76e80386b', N'60fd76272a7ed4e8ea142efe78b67cbf', N'4a5607be2e2e8a92305abcd2b7538d4f', N'2017-02-22 15:08:10.290')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'612b98fdce94828294086229184a308feb493ceb470bc55d27f42b05fb083a4b', N'KFcpXX4aLkWdhrRU58oydg==', N'05959cf0e9e340f4bdc96cf2f9229abd', N'612b98fdce94828294086229184a308f', N'eb493ceb470bc55d27f42b05fb083a4b', N'2017-01-13 10:57:33.120')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'612fd4c81b4efa11bc6e1e700538ff987e6b9a22429ac1f23cd0426ed1b13283', N'QNNxa3c4W0azuhj7HFspig==', N'092cbac4cc814200b2bc17912968175d', N'612fd4c81b4efa11bc6e1e700538ff98', N'7e6b9a22429ac1f23cd0426ed1b13283', N'2017-01-04 12:21:44.300')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'6168f6b4e993f056c07e6b6c93ef2bd283b974877d3ee7fab7115b82eab557c0', N'8rCWZ7V4ZUKB125Gnf7n4A==', N'c64251e56dc8484dbd30335a6d8b3e99', N'6168f6b4e993f056c07e6b6c93ef2bd2', N'83b974877d3ee7fab7115b82eab557c0', N'2017-02-18 21:49:23.143')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'61af09119684c0e7d1d077f3bbea2850098b5be8755ff5b70d6e2c5427d141c4', N'sPUTk6nJSEmg65OeC2tADA==', N'bdebf142858941fda296eb671e6b18e2', N'61af09119684c0e7d1d077f3bbea2850', N'098b5be8755ff5b70d6e2c5427d141c4', N'2017-02-03 12:33:21.490')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'61d471388bc532b74d6ee6c2dd706d6ce2adfff7f8516468c21c4d78d56bc990', N'bfH530LUiUCv4wcOempUzg==', N'8e5317deecd247c2b241d5664df744ec', N'61d471388bc532b74d6ee6c2dd706d6c', N'e2adfff7f8516468c21c4d78d56bc990', N'2017-03-06 14:51:02.523')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'626a27c61d0b65b9b24f933f0d0f8443e981b32ed6689eb34087f1b5ab7398b6', N'DO0GewGZWkq/5L29QJJcAA==', N'ffed2ab73a5f456d8b9760468d1dafac', N'626a27c61d0b65b9b24f933f0d0f8443', N'e981b32ed6689eb34087f1b5ab7398b6', N'2017-01-19 10:01:09.177')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'62f07cd704e7a63a94b50b2af0f1f55515d4c6b3fc87ee810e43c141d18c3c62', N'0mafoOfTsEaWUVfwN1gLPA==', N'7b8d2e1f613840bdb2ca3a1734597980', N'62f07cd704e7a63a94b50b2af0f1f555', N'15d4c6b3fc87ee810e43c141d18c3c62', N'2017-02-18 20:49:24.437')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'62f41294a0c121aa6b45625bf9c74c8c9af30757ee5deb042a86c73b36dd64d7', N'rUloOntAIUO8UhAQ0eq7ew==', N'bc486bd918d3479eb1975212c60867f3', N'62f41294a0c121aa6b45625bf9c74c8c', N'9af30757ee5deb042a86c73b36dd64d7', N'2017-03-01 11:08:45.587')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'63e6e6cc5b89b3f78aee4a655184ccafe5c8f68f7b56271939bda8ab13b7755c', N'R54n0q5iPU6QCRM5BFet1Q==', N'4b9ab5051d7f42debae89d392ed0dc73', N'63e6e6cc5b89b3f78aee4a655184ccaf', N'e5c8f68f7b56271939bda8ab13b7755c', N'2017-02-08 09:52:11.457')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'644a95678b3834b3e4afe1c4d743bb03cd7d264e1b080bf34af85b48b887356b', N'nAiVaBKP60KIfrZ9prJL3w==', N'3f4954359a3e4107a38d34f427d153f5', N'644a95678b3834b3e4afe1c4d743bb03', N'cd7d264e1b080bf34af85b48b887356b', N'2017-03-24 12:05:06.897')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'6455a245c5911e237c26bb8f564cd46d26e297b80e8898beba36ec304e91a9ba', N'UpIYqYMEwE+6j8yZx0viHw==', N'7258779b18cf49e5b6ac16c4a8154325', N'6455a245c5911e237c26bb8f564cd46d', N'26e297b80e8898beba36ec304e91a9ba', N'2017-03-01 00:55:12.710')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'654594fff873871a56d7e4dd39cb29e1d12a551bef9f83545a0776619e19a898', N'bHGUkmB+okuiVU/0z85GJA==', N'55c1ef8803cd4b249f3e03515af6835d', N'654594fff873871a56d7e4dd39cb29e1', N'd12a551bef9f83545a0776619e19a898', N'2017-02-14 11:57:54.107')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'655ab3b12876a85a8864f5253ae3377525adaf13bb7e7f40fd5f478944030b75', N'ctJFxgXh8U6xo2WFzk84Wg==', N'a9180a2339014aea992d91338d5e85da', N'655ab3b12876a85a8864f5253ae33775', N'25adaf13bb7e7f40fd5f478944030b75', N'2017-01-12 16:20:05.180')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'6662ac011954cf60ca63b6aff817a82770da52a066ad2aa64d2c2c8d5f878d42', N'7erJn66RHkGSmEeqNcw/Jw==', N'000e3be49f14406fb25b4f261c26269f', N'6662ac011954cf60ca63b6aff817a827', N'70da52a066ad2aa64d2c2c8d5f878d42', N'2017-02-06 15:08:40.167')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'6665b1f1a5efd331ee79dcd16f6e2af4fac22af6dff8596388dc93c2297dadc9', N'lB+KV5+gjk2PeaO28uQ8yg==', N'0f755c1242144fff8b8739e58ea749d8', N'6665b1f1a5efd331ee79dcd16f6e2af4', N'fac22af6dff8596388dc93c2297dadc9', N'2017-01-19 15:35:59.107')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'66e0a3ef21e3e97a0d37edf488f560bc338408b2ac772fb98cf713d8e4a85715', N'3H44M4mEeUSEcJuTelI7Rw==', N'fee99db666f2491485cd83f39983eaca', N'66e0a3ef21e3e97a0d37edf488f560bc', N'338408b2ac772fb98cf713d8e4a85715', N'2017-02-09 22:54:23.737')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'67e0673b5c4b121901bddbb67e79de480e68ca148d260e50ed361071344c21fd', N'0DgWg7TWAEiJ2lziZAluoQ==', N'5259d98845f240508f14101311fd500a', N'67e0673b5c4b121901bddbb67e79de48', N'0e68ca148d260e50ed361071344c21fd', N'2017-01-18 10:17:20.017')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'692b6adfb6b31c692a3236e448401494429f8db4004cd4d80c00395c1a553fa3', N'R10xDOIY2EuhAKQi1b9kAg==', N'1955b1c0e1c345a3b36ebd101f1756fc', N'692b6adfb6b31c692a3236e448401494', N'429f8db4004cd4d80c00395c1a553fa3', N'2017-03-28 19:42:26.983')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'69ff204ad5270ebc3fc80773636c12c955acd4988169bf4815e851c4b47ca9e8', N'3QMtV8m1m0uf2nO0Rq2yIw==', N'ff429cfa1f0a4a199e1f8500b1335eef', N'69ff204ad5270ebc3fc80773636c12c9', N'55acd4988169bf4815e851c4b47ca9e8', N'2016-12-30 11:10:15.460')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'6b1351e21a5c89663ec1fa1508887398a1730e2fe4d433700b90a9e34fe8fb7e', N'U+MVLSIBqU6aoEzKWBlpJA==', N'6014ee8f2c8b4669bfe70dcaf0a0760e', N'6b1351e21a5c89663ec1fa1508887398', N'a1730e2fe4d433700b90a9e34fe8fb7e', N'2017-02-13 18:09:14.950')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'6b3930e1dd5464e11488d57bf7e6f87b10ade3690706cd0ce02d357da735635c', N'/7wlU418qkmXiQUnBU3XrQ==', N'93ebcaeb7c2947e1b073c407bc32aaa4', N'6b3930e1dd5464e11488d57bf7e6f87b', N'10ade3690706cd0ce02d357da735635c', N'2017-03-22 17:40:08.547')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'6cad95a57236e5bad8548ea9fa4486bcce642a1ca2707346f29cb96d4ffe9685', N'LAJXwDgQNkSQGfpdUmorRg==', N'cec983171f9e4bc8a4d4b2cac84c3cfb', N'6cad95a57236e5bad8548ea9fa4486bc', N'ce642a1ca2707346f29cb96d4ffe9685', N'2017-03-03 18:17:27.473')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'6ce1001910a4f26849daefcf92b59925f3fabd4c43e6b193f772f53bbde2b6da', N'Ts3x1gTVAk6Rz2zY2L6i0Q==', N'7452d3008f644c3ca4ad105983d9538c', N'6ce1001910a4f26849daefcf92b59925', N'f3fabd4c43e6b193f772f53bbde2b6da', N'2017-03-24 23:50:47.163')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'6d4ea5c7c7327606a33280aaf1079cc463611700e0c1f5172fcd73a043f0bbc8', N'5YE3AWynGEqYH18UTxAS0g==', N'910924722a114fc3aecfa6a1df302f03', N'6d4ea5c7c7327606a33280aaf1079cc4', N'63611700e0c1f5172fcd73a043f0bbc8', N'2017-03-22 21:55:27.260')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'6e232f30d1eda71e02710ebaa8d8409517c3e06b79b53ce438e738923fc2c6de', N'JVn9G3QCJkSnAQRM+LmkeQ==', N'98bf1b079cca4ea6b6f5ac7a729a2204', N'6e232f30d1eda71e02710ebaa8d84095', N'17c3e06b79b53ce438e738923fc2c6de', N'2016-12-27 10:10:20.167')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'6e6b163dd4ec2f696767e92e49796e1e5a3e361191babc0b8aa8a01cf1ea703e', N'QNN0fKLZ/U+XNrfYAI007Q==', N'03812345bce84debb5a4787398395bb9', N'6e6b163dd4ec2f696767e92e49796e1e', N'5a3e361191babc0b8aa8a01cf1ea703e', N'2017-03-15 13:52:19.903')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'6f278a34e2dd27d69af55a6f125d733f7f8f2d82e2a228d11c47753bbec01bdd', N'voJi2U+Mak+mZLYFpMdmoQ==', N'4481a02eed1f4be68c0c5576a47baddf', N'6f278a34e2dd27d69af55a6f125d733f', N'7f8f2d82e2a228d11c47753bbec01bdd', N'2017-02-23 10:02:11.037')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'7015000e7675d1bdf1a59cb7b1b1f19bc8a4f66215cae045a22f94e99c047c98', N'0h598jYv9EubOeuirIGkyg==', N'bc4455f01adc4d33800ad09938e1e219', N'7015000e7675d1bdf1a59cb7b1b1f19b', N'c8a4f66215cae045a22f94e99c047c98', N'2017-03-15 11:53:58.247')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'70237cc0d7f085d017fda3a896575267e2b8c8580dc5a72aa49882781552890a', N'P4y8V4jKVU2ma783yuhAVA==', N'cfa10a46a1f343d88aa21c15c1350295', N'70237cc0d7f085d017fda3a896575267', N'e2b8c8580dc5a72aa49882781552890a', N'2017-03-21 18:33:49.603')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'7025405f529cde29a6145aff1ed0d37da73681973563aed0fa9ce27b857f439a', N'JoAABN7by020zJHHw+fcJg==', N'88edd5119f564ae2b0d009ddc7ef495a', N'7025405f529cde29a6145aff1ed0d37d', N'a73681973563aed0fa9ce27b857f439a', N'2016-12-26 14:24:40.177')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'70887e6f77b34251db5e29775ece7858b01f9becbd3ba7e1c593dbc8889c2744', N'UGpiukbwREe7ookRXpZpnw==', N'a22495906c204f21b2cda217a1e218c6', N'70887e6f77b34251db5e29775ece7858', N'b01f9becbd3ba7e1c593dbc8889c2744', N'2017-03-23 16:54:55.650')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'715b5607e3b28c663d74e68bc3b4c9d690026510865f4079d7a7df6d4b004c34', N'uq4E8dT6rUCRg8kpSuw3ZA==', N'3fe15a061dbb4bc4b63dd8ff2136ca3c', N'715b5607e3b28c663d74e68bc3b4c9d6', N'90026510865f4079d7a7df6d4b004c34', N'2016-12-28 10:25:16.960')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'71a7352b60587b8f8f1d3335c5248e709740110e0573ad0d470c217f4602181d', N'ktjBoWYGBEu1icZOvynmwQ==', N'd572e502f9194d7a8d9983fd70a73b99', N'71a7352b60587b8f8f1d3335c5248e70', N'9740110e0573ad0d470c217f4602181d', N'2016-12-27 10:12:20.073')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'71c02383d73c1dcebfcd8300678d5e38b2e60c84d936c32d6cd84e507e9b1b3c', N'uQKZSsBcF0qid4/rV05UfQ==', N'353d7e3d1a3a45a0a46819278db6df4d', N'71c02383d73c1dcebfcd8300678d5e38', N'b2e60c84d936c32d6cd84e507e9b1b3c', N'2017-03-27 14:23:16.233')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'71d8742f9dac35151fa963c33b459960b6f355428dba15c6cf510ba00fe5d380', N'mjeN4UBVJkeSC5dEUZlaTg==', N'b3d0b028f850493792a36017bbf4951f', N'71d8742f9dac35151fa963c33b459960', N'b6f355428dba15c6cf510ba00fe5d380', N'2017-01-12 09:21:38.553')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'71e6acc81f6528a8682e56a03c0fd98a2c79d12ed1e44bf9b819dca7f574b68b', N'eWpiesK6C0evFCQZ5pLGsg==', N'5ddb3e137f9a4c6d90a18fa08cf15c19', N'71e6acc81f6528a8682e56a03c0fd98a', N'2c79d12ed1e44bf9b819dca7f574b68b', N'2017-01-17 17:25:18.197')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'72ef5147d2de3a285f3a7afd4cfbb964ab56a78b71cac9ef821a25f2f1d7bce8', N'SUMAB0n9rUW36aGgq/J2sw==', N'eebf08e55d0f4f6da20f63cd61e896a3', N'72ef5147d2de3a285f3a7afd4cfbb964', N'ab56a78b71cac9ef821a25f2f1d7bce8', N'2017-02-01 21:19:58.040')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'72f873e872655a7737dbc0c3a0d32d1feffc9a66bcb983a9d470823d7abaaf54', N'9oVYSZkIN0SptPL0GOkDpg==', N'f32584e0a53c45ada8d44ac7ecd357c5', N'72f873e872655a7737dbc0c3a0d32d1f', N'effc9a66bcb983a9d470823d7abaaf54', N'2017-02-23 16:37:29.150')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'72f91850cffd139e88c6a37e384d531083d274bfd61ffeb9bd45249eaafa539d', N'51a+mCgs90elFQjXtwtkGA==', N'674de36086c04c669e647aeb2aee95cf', N'72f91850cffd139e88c6a37e384d5310', N'83d274bfd61ffeb9bd45249eaafa539d', N'2017-02-26 15:30:53.947')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'73b85167ca5a6bc9c80e43e97559d476719d2e8366f1a80ab0e725b21dcc5630', N'HPcKwo6tlUm3fz6YWE7Nhg==', N'ce3fc15d8a1c483788eab3be6380db82', N'73b85167ca5a6bc9c80e43e97559d476', N'719d2e8366f1a80ab0e725b21dcc5630', N'2017-01-17 14:44:59.913')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'7421a6f4614be74c6111cb29442428b678594e965d834cd8c4375b50c5201a6e', N'gwnefslIdEO2xZNlcLFgMw==', N'de35c3303e65495caee88364b27ecf08', N'7421a6f4614be74c6111cb29442428b6', N'78594e965d834cd8c4375b50c5201a6e', N'2017-01-09 10:24:58.827')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'7452fc0919d5396d491ab42c835755e77b3078071b2f836d9291d142711b3d13', N'6RcITxBZ30izsDyjlAumnw==', N'21135d205b8341a58226e5882af10546', N'7452fc0919d5396d491ab42c835755e7', N'7b3078071b2f836d9291d142711b3d13', N'2017-02-14 16:12:50.040')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'74f339e010ebebc5e648ada68e62af50c8cdb5393fdbb3a5505f26d414841c87', N'X4x3I2nfrEGTIKoXnMFEsQ==', N'059a4c627f0649948b870e92deb750c1', N'74f339e010ebebc5e648ada68e62af50', N'c8cdb5393fdbb3a5505f26d414841c87', N'2017-02-08 11:59:56.633')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'759418c5ecff0703da4b9bf01f93cceef4c38294610e4b543fbdc15370522ab3', N'a4h3an6eO0254xfAo76+VQ==', N'340f891fafd145d192fa853e20e4e30e', N'759418c5ecff0703da4b9bf01f93ccee', N'f4c38294610e4b543fbdc15370522ab3', N'2016-12-28 16:02:11.107')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'75bca56c9a8f79425c5f10d9fb6510809cc0b8866738e02b1864dca52c129ced', N'gBtklsMEw06B0WdwI1TakQ==', N'593686e2bfb141868bfd088f189f02c7', N'75bca56c9a8f79425c5f10d9fb651080', N'9cc0b8866738e02b1864dca52c129ced', N'2017-03-24 16:38:12.237')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'767f44cd9a87713f9ace6c36e1000e35bee3a600c78214c942f91a7c76662a39', N'fFkqErDnKEWJuiCtugFpCg==', N'330f9d7aca6f494fa8ee2c14639122df', N'767f44cd9a87713f9ace6c36e1000e35', N'bee3a600c78214c942f91a7c76662a39', N'2016-12-27 10:11:45.943')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'770cd3c35e8ea5d6cc6b87dd2b04633c80bf681e299496476d6846ca017ea1cd', N'8alp3h43DUSHxtK7rx8tNQ==', N'4ed10e7573924dcca75b51c1a7f24408', N'770cd3c35e8ea5d6cc6b87dd2b04633c', N'80bf681e299496476d6846ca017ea1cd', N'2016-12-30 16:59:03.637')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'7864e61e9fa3d467c6fd7a4f5d71075c6724d4329c6c7358d7508750ffcd9d28', N'KLyc07jLq0qXjUyhDRXp6g==', N'6133f3cb9de14c3cbabb4330160a640c', N'7864e61e9fa3d467c6fd7a4f5d71075c', N'6724d4329c6c7358d7508750ffcd9d28', N'2017-03-21 15:30:21.913')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'78e916e21ee39c106f39674b3f5764b78272539ceb1132e37d8c8e23b0263563', N'lVqfpV8lVEWSHcuMeXdMeA==', N'e73c9f58ae4645279e3cd72b3a1356ad', N'78e916e21ee39c106f39674b3f5764b7', N'8272539ceb1132e37d8c8e23b0263563', N'2017-02-26 15:26:18.370')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'792bb274bbd80eb73bebf268c206b96836c113d5d848d642db2de0bbc761cade', N'A5WiUAsoOUSLnow5NrsbZA==', N'8cd126844fbb4da0b56f941755835468', N'792bb274bbd80eb73bebf268c206b968', N'36c113d5d848d642db2de0bbc761cade', N'2016-12-27 10:12:16.027')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'7a5ab4c06ab04fe7a162ead78575204004cdd5f6cd55cd3e82a0b70778a6cc8d', N'FGgS3+kL3kuJ6z3BiihjKw==', N'8d428e6847e14700b24e5ff8edbcbdf2', N'7a5ab4c06ab04fe7a162ead785752040', N'04cdd5f6cd55cd3e82a0b70778a6cc8d', N'2017-03-22 12:30:19.000')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'7adcfa3c942a9aefa86f929ee5b2c3b2d1b0db0820b3cde0b47d9aa3aa9f6b1e', N'/9zmHqeU5UezNYtlVFNw7Q==', N'f692444be5e74cdebc37e2c527658da1', N'7adcfa3c942a9aefa86f929ee5b2c3b2', N'd1b0db0820b3cde0b47d9aa3aa9f6b1e', N'2017-03-21 14:36:55.623')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'7b7492c3e8cac831c0b934753d023c127b744f6babc958def063d927611fc3c6', N'MBIERRgRu0SUkPNGBByNtQ==', N'f3b33e3928b74f078931a42b625a34f8', N'7b7492c3e8cac831c0b934753d023c12', N'7b744f6babc958def063d927611fc3c6', N'2017-03-03 18:19:55.383')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'7b7b33d27585dd5d068ec50326adcb081ad1a743d6797ba71ef89256a205072d', N'FKSuTc+liEeorhjoaNk7JA==', N'82198047a28e48cf9957fac9fd8cded9', N'7b7b33d27585dd5d068ec50326adcb08', N'1ad1a743d6797ba71ef89256a205072d', N'2017-02-23 11:03:32.410')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'7c3629bf9722ab5ffc0cc070a3cb743ff4dcf38efad9c2b78a210fd2059e855d', N'0tlqohYsvUCN1LKsZYHV2Q==', N'ea202a62fe734814943e481e33030e9f', N'7c3629bf9722ab5ffc0cc070a3cb743f', N'f4dcf38efad9c2b78a210fd2059e855d', N'2017-03-09 11:07:00.330')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'7d5290842b7dfa3c71fe77c5a3dee5e4850607b1bfb25cf3fd67035474ec4354', N'bZRk7Q9OjUyaivT7Cpi6ug==', N'53b54bbc5bb04046bcfd120c4ac3bb32', N'7d5290842b7dfa3c71fe77c5a3dee5e4', N'850607b1bfb25cf3fd67035474ec4354', N'2017-03-06 20:14:46.030')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'7d57e31eefbb45e217833c09cc64dabd30a9488adf72af5005742ab50d513e6d', N'JvLc2LMLWU6oBrqGYT5i8Q==', N'd20074314cd24748b3ef8ba645d34b2a', N'7d57e31eefbb45e217833c09cc64dabd', N'30a9488adf72af5005742ab50d513e6d', N'2016-12-27 15:57:34.787')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'7d6bbac88935ad0cf1135d598c683c13c697c37fcfdcfda8035298b295d08d16', N'kZjUwbJw0ESGyaambgK0Bw==', N'6728fe7032e2411cae0d92afaaa442b3', N'7d6bbac88935ad0cf1135d598c683c13', N'c697c37fcfdcfda8035298b295d08d16', N'2016-12-23 22:38:17.987')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'7d8275cba61152f070581519aa36b8e03a302d2ccaa17b6c5fdd2303c34e9b70', N'FJb/ILKJ2EC7W3Jq0P59Qw==', N'188421af416e4f9380dfdb1ccdb66dda', N'7d8275cba61152f070581519aa36b8e0', N'3a302d2ccaa17b6c5fdd2303c34e9b70', N'2016-12-27 12:55:28.490')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'7dda60ff557da7fb2df9321977d10530003c1e25e52c594ccbee521edffe5c7d', N'9N20XpFLNEeeNO5P4Lha+w==', N'161fd2d40eb14a038ef036bfb750ea95', N'7dda60ff557da7fb2df9321977d10530', N'003c1e25e52c594ccbee521edffe5c7d', N'2017-02-23 16:14:42.887')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'7e1617150afe775476bbf3b4e490ef248a56d860d2752d6f8c29bc10110fa343', N'rNc+0RWhpkm52V82z67ChQ==', N'201099513187426e84c6213b4169ba31', N'7e1617150afe775476bbf3b4e490ef24', N'8a56d860d2752d6f8c29bc10110fa343', N'2017-03-10 11:41:33.073')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'804ccc7d1c3303a4e52c25d66dd42f3dfc411f75b97d2539da7068c63e7e7a3d', N'xItvEtOj9EC/gv2urB9SmA==', N'941910a1064b4f52874d18b8ecb13d1b', N'804ccc7d1c3303a4e52c25d66dd42f3d', N'fc411f75b97d2539da7068c63e7e7a3d', N'2017-03-21 23:45:29.860')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'80a2d6016185eb43ea19fb126298ba3536422e309091ea58f5300fb3568de2dd', N'bNOwdYJeY0C1KlW8Prbkmw==', N'2af7d8afc04b41108c190859d5060d34', N'80a2d6016185eb43ea19fb126298ba35', N'36422e309091ea58f5300fb3568de2dd', N'2017-03-09 16:43:25.750')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'81f223a53c3951b89fe8a06b9f1738875da0f70ee2b04dfe1de48a53ccd1d8dc', N'xjmA8NZ5skKRAftTnppOow==', N'2d91cf88805745b38cda78d5fc23915e', N'81f223a53c3951b89fe8a06b9f173887', N'5da0f70ee2b04dfe1de48a53ccd1d8dc', N'2016-12-28 15:43:11.487')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'82484917f7cd59b3a626decb913fd46759fbb6dc3482ea34a49b52b87c7d4e25', N'3t5Xst2cGEGD+QljvQhVMw==', N'8a9b891d07924265a7926dcdbfbb02a6', N'82484917f7cd59b3a626decb913fd467', N'59fbb6dc3482ea34a49b52b87c7d4e25', N'2017-02-27 20:51:43.880')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'8297ab5198e323a8240f084356ea6313052d8bda2f7485bc276e6e0cd0daf886', N'YAEyhbYYkE+Z9t26hsq7aQ==', N'c01637319a2e4c36b92ca1d299b753b0', N'8297ab5198e323a8240f084356ea6313', N'052d8bda2f7485bc276e6e0cd0daf886', N'2017-03-20 17:44:40.943')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'829f2ff8ea738f0b1c107bcaf3659788ac5989ec2ac817e895421d5ec9a1ce66', N'611eAF5M/kG3QFNpqX+7tg==', N'50c8763fcf8b43e8b6d655a372a3de88', N'829f2ff8ea738f0b1c107bcaf3659788', N'ac5989ec2ac817e895421d5ec9a1ce66', N'2017-02-20 15:31:34.093')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'836f31354f17c85b300de3667f51ba100c0d02dbbd9a0916b2b275e8191cdbe1', N'lUrTVpfZtkCeen8x8b4lmw==', N'b1efd468fec340dcad0333b4d0086ca4', N'836f31354f17c85b300de3667f51ba10', N'0c0d02dbbd9a0916b2b275e8191cdbe1', N'2017-03-01 14:21:50.807')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'837b4971b510291a2d58451662b43b6a280148161c86bd811d239b5392f0c842', N'TQ4UPN96n0uHpAJu4nYU+g==', N'cdc2d2aac88645779871ed7b70cc4ca0', N'837b4971b510291a2d58451662b43b6a', N'280148161c86bd811d239b5392f0c842', N'2017-03-17 18:25:45.123')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'83c9183db6aab77931eb4397a34659e86fb991dd0a922054958e8d3114b61f09', N'GEYcLP06vkevSCHQMNWGIg==', N'b607911295d8442d91f7b81e5038a493', N'83c9183db6aab77931eb4397a34659e8', N'6fb991dd0a922054958e8d3114b61f09', N'2017-02-23 10:29:38.857')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'846d87b10e5b5d9d3dcbe162dd97251b01e1af394709a0dffc2a1ef75c1d0d2c', N'rHG5oWogUUq6KP74LuV/5w==', N'd27f44aae5aa41cc82b5fd5f2b3c7437', N'846d87b10e5b5d9d3dcbe162dd97251b', N'01e1af394709a0dffc2a1ef75c1d0d2c', N'2017-03-15 12:21:22.780')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'84d2fd0164dc014c88acba144e2a13771be34414f83f60d78558fdb2a4ddfac5', N'YhC+HEB1hE+qBz5IArVKvQ==', N'e8da23f9df644481a173bfc54b618fa7', N'84d2fd0164dc014c88acba144e2a1377', N'1be34414f83f60d78558fdb2a4ddfac5', N'2017-03-01 14:52:12.230')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'852a5b0b63be85b375973e92ddc07cc12291807bbe09607a893d12277f44d58b', N'S+YMfYcSQkWuBweBgEzYzA==', N'6976b275363d4a34883f8526ccf58914', N'852a5b0b63be85b375973e92ddc07cc1', N'2291807bbe09607a893d12277f44d58b', N'2017-03-14 19:54:54.763')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'85b25f1fe93f1e99e9e6270ed696da597279597dbf1cc8b8c22a4904dc8da176', N'57+ie8f36EqexmxNlZ0+FQ==', N'778eb64e15874f6490c1a20eaf2d777b', N'85b25f1fe93f1e99e9e6270ed696da59', N'7279597dbf1cc8b8c22a4904dc8da176', N'2017-03-20 16:58:15.967')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'87176e65950c3886cefe87fe4acf7b67a47e2704c3dc70ce05aa00ba98f13829', N'jYhVIiR3AUKRd609370V3A==', N'6342ec822b2547d79fe8b0cafe3016da', N'87176e65950c3886cefe87fe4acf7b67', N'a47e2704c3dc70ce05aa00ba98f13829', N'2017-01-13 14:04:28.623')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'8776bc2188fa889a95ac0ab738675b936eaa050ae2614a547db7a44da8ded3e4', N'u0TFS81WC0WQYN7j5xdDjg==', N'817f8f0667a6467b8b6c56414a23686e', N'8776bc2188fa889a95ac0ab738675b93', N'6eaa050ae2614a547db7a44da8ded3e4', N'2017-03-16 16:35:22.407')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'87a97507f594c6cf7dacaff55ecbb35a630444a24b929a36c3f4e7b8b6d6a66f', N'X7pr0UAr9US3CMGEK4jJeA==', N'90b39b41cba144d8b0fead7e99810294', N'87a97507f594c6cf7dacaff55ecbb35a', N'630444a24b929a36c3f4e7b8b6d6a66f', N'2017-03-01 16:59:02.063')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'87e0ed66f537287b5552b27806c84edc287097371fb411c6fe97b55566276272', N'C8DfS8lKfUSWnDHpq+hS6w==', N'8aa2fa7ce0b144f69e7c92765164384f', N'87e0ed66f537287b5552b27806c84edc', N'287097371fb411c6fe97b55566276272', N'2017-02-15 23:28:11.463')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'87fd00cc0d36f21ff7224709e7bdffb7cf949f6af7c5686059303e3d6608a968', N'zMo3YvBeIUq0RaxnEA808w==', N'fad8badd01af4d88a999e8424ed40d60', N'87fd00cc0d36f21ff7224709e7bdffb7', N'cf949f6af7c5686059303e3d6608a968', N'2017-03-20 11:09:10.363')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'890565c6ced546d4684d7858b8cd5af44e6befcbd80b419dda2f50b508e10973', N'PREIOwIoMEy9ZJMVWdTXTg==', N'31ef5deccb62407796b9de06bb4f39ac', N'890565c6ced546d4684d7858b8cd5af4', N'4e6befcbd80b419dda2f50b508e10973', N'2017-02-25 22:57:49.367')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'890727587324430ca282c209c6f58030876983df2fe62f41cb96dda750fae7a2', N'Z46HH6hjz0e5IGH+fVMD/w==', N'81ff5a1e581f45169dad91ee491d02b2', N'890727587324430ca282c209c6f58030', N'876983df2fe62f41cb96dda750fae7a2', N'2017-03-06 17:20:17.913')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'8990347bb7d2e92dda4c4ef3a83dec08c863de67beebbaf4a20ec61c52c16968', N'jP1xdbXiaE2D/3CK0dCHpw==', N'bb7fd978edc042a5a74f4daba7747f77', N'8990347bb7d2e92dda4c4ef3a83dec08', N'c863de67beebbaf4a20ec61c52c16968', N'2017-03-17 17:45:43.017')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'89f506b8192be615334f91fc7e3602981c046563f729ed8c5971bd439b34a510', N'JxshMw9KAkOJN/PIwHR+nA==', N'982225647a2f4f3d872835bcea21aef9', N'89f506b8192be615334f91fc7e360298', N'1c046563f729ed8c5971bd439b34a510', N'2017-03-28 17:00:43.480')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'8a4b63aaf31d975fd9fd1aa1df4849a3a4648c3bb2dce1bb5f53fdbc1fa590ca', N'9a+lL477w0e/Miq2QuuF7A==', N'2a656d14b52d49379ea8c5e546d0f029', N'8a4b63aaf31d975fd9fd1aa1df4849a3', N'a4648c3bb2dce1bb5f53fdbc1fa590ca', N'2016-12-28 16:16:43.797')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'8a71d0cd204c340de6cd58ad6227dc2a26d42fd1e77dcff1a19b18aedb5f2259', N'Br9if8ZZOUWdNSBjB3Cu4g==', N'810be7934acf48d785c2b864be1483bd', N'8a71d0cd204c340de6cd58ad6227dc2a', N'26d42fd1e77dcff1a19b18aedb5f2259', N'2017-02-23 10:03:30.517')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'8ae475fe9610698886aeec33ca2a31242d8138decee62c3f61fd16f10ccbaf1c', N'UdjiptfgwUuMhUEqKNrHSA==', N'55882707ac0e44c1b876effb2bd2e4bd', N'8ae475fe9610698886aeec33ca2a3124', N'2d8138decee62c3f61fd16f10ccbaf1c', N'2017-02-13 10:29:33.920')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'8b20c31c0089e49700bf67e675cf5e44aae51b3d82f86715658a99430e1b5bef', N'jQjoQuIm1k6NvMvOcDUr9w==', N'df75496080994a698238fe97e6cf5c74', N'8b20c31c0089e49700bf67e675cf5e44', N'aae51b3d82f86715658a99430e1b5bef', N'2017-03-09 11:37:21.273')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'8bfdb2ee84233f54695e48d6a3232509ef1e0c7a3798c2047783d5de05e9d6bd', N'XKgLyFArMkq3ABR3tfLF2Q==', N'4d2b0872c57d48e7afdddd173ea44acf', N'8bfdb2ee84233f54695e48d6a3232509', N'ef1e0c7a3798c2047783d5de05e9d6bd', N'2017-03-21 15:44:50.167')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'8cfb3a27eafad1c77d50944d8a4178c5a6fae47d8a9e5fae14247b5e69d7deab', N'k7qEDpriHEOf4jEvyVan4g==', N'af4f9105e1a9468b8ac862185db27e2c', N'8cfb3a27eafad1c77d50944d8a4178c5', N'a6fae47d8a9e5fae14247b5e69d7deab', N'2017-03-09 11:40:47.847')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'8d83774a649faa9aafa67a6911f51a53bf162e39f9e4b542303d1e390b5d858d', N'bEiFssaw4EOMai8kkbRdcA==', N'bc1e7420c0444511a920ac7849292ee5', N'8d83774a649faa9aafa67a6911f51a53', N'bf162e39f9e4b542303d1e390b5d858d', N'2017-03-16 12:17:48.543')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'8da5ab7b248ac9391bc365324b105ee09fdc5f877db0aabf580217326ae35159', N'lQ0Ft95KokW1m7+CmIdjYg==', N'040bcb71719b4b87b3976ef70cb923c6', N'8da5ab7b248ac9391bc365324b105ee0', N'9fdc5f877db0aabf580217326ae35159', N'2017-03-08 14:54:53.070')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'8dca9545fc52a7c94f3a4ec467b066656451f7e720a8c97b114e8ec21559c096', N'mBEMUcc2oEWgM5TwX4on8Q==', N'543f2817d9054243922163817a553a3c', N'8dca9545fc52a7c94f3a4ec467b06665', N'6451f7e720a8c97b114e8ec21559c096', N'2016-12-22 14:55:56.347')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'8df48251b3bcbdc51ab58cae8ff24692f16c2efd6c612103c571eec70f45a45b', N'fEzZE4j4+0Gee6EiRGNj7Q==', N'e65e091b4b2749aa9045b581d0d44afc', N'8df48251b3bcbdc51ab58cae8ff24692', N'f16c2efd6c612103c571eec70f45a45b', N'2017-01-12 19:37:17.270')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'8eb6803b114df9221fbe8834cb154518d4bb82ab3d7dcc354dfa901acc9fba82', N'4X0dr+uSU0C3msN9B30LWQ==', N'541e01243e1549f094a8f9a88c38f318', N'8eb6803b114df9221fbe8834cb154518', N'd4bb82ab3d7dcc354dfa901acc9fba82', N'2017-03-27 12:02:38.617')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'8f0a18c7c7add2d507f30c2a92738ce53ad8a6ea092832183b90bd37670b4578', N'pD2zqUY7wkOMzyS/1REIIA==', N'b14a67b0973244a1918afa8d68918302', N'8f0a18c7c7add2d507f30c2a92738ce5', N'3ad8a6ea092832183b90bd37670b4578', N'2017-03-08 12:22:44.197')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'8fb50a22679d289bf4237970cc5b9dcb55fabdf20564d47101941f0c59e078e6', N'CZGVuj5Ndk2k15yBI2YA/A==', N'c985d0f71a244270b6661bb2778ca745', N'8fb50a22679d289bf4237970cc5b9dcb', N'55fabdf20564d47101941f0c59e078e6', N'2017-03-18 16:11:24.837')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'90d423f5a3010c1a8fb3e86cd1ef1575c6327e7abb27c8fa4467f6641395cd7e', N'z6piprsQtEeW19D9sjkLLQ==', N'e974891757654a3b9f08a8f328229102', N'90d423f5a3010c1a8fb3e86cd1ef1575', N'c6327e7abb27c8fa4467f6641395cd7e', N'2017-02-20 19:25:40.560')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'91bec5b282baf9716bb6bf5f012a73e7a87e5b0aff198b3fad69f3029b6b1ca3', N'VxJn3jA27EWD1AKk0wNFpQ==', N'136612e8327e4beda3040d093b93c1aa', N'91bec5b282baf9716bb6bf5f012a73e7', N'a87e5b0aff198b3fad69f3029b6b1ca3', N'2017-02-23 11:17:10.907')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'9213afce52ccc003ba1be05a6746a199fc262a6c2d0739991c86fbb2cf09aa63', N'aI66UQ2rD0GBrAJRH67H9g==', N'847579d076ec4b83969b9f703e1f1d41', N'9213afce52ccc003ba1be05a6746a199', N'fc262a6c2d0739991c86fbb2cf09aa63', N'2017-02-08 17:56:14.173')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'9220e7c87d3bc317fd39200a208add07bf209d0437bb70134c29fef15e4722f8', N'Bq7LDtCQsE2Q5OcKwDfvXA==', N'395b664796b04bb78997e566418cee38', N'9220e7c87d3bc317fd39200a208add07', N'bf209d0437bb70134c29fef15e4722f8', N'2017-03-01 11:59:31.307')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'92d2fd14e87d1869cc23e32f0aaa6de239682806a17ed0ddefb15652ead70f78', N'gBYPTmyXnE6l0tqJhJ33oQ==', N'ab25538565464dbca5b4d577c6d05884', N'92d2fd14e87d1869cc23e32f0aaa6de2', N'39682806a17ed0ddefb15652ead70f78', N'2017-02-20 17:44:04.940')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'9408679f7f1afa8a933ef5b434000b393123a87d28bf3bc1da7a5f464da62512', N'Df1PggBrXUqW7uxqy3NnMA==', N'1e01248a3038402d9d9f9200edc911f8', N'9408679f7f1afa8a933ef5b434000b39', N'3123a87d28bf3bc1da7a5f464da62512', N'2017-01-04 18:37:19.373')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'941d48c7559e99bbd783fb97faa911a564d907a0cd808148d6b35b4a3a9b4ad4', N'qUodBN4TD0mF7lIUlzEc4A==', N'b2cde25ea63d440dae735f11eea883cc', N'941d48c7559e99bbd783fb97faa911a5', N'64d907a0cd808148d6b35b4a3a9b4ad4', N'2016-12-27 10:11:29.957')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'94c045a17327e0caf938b486b7f3cd4b434bb975fde1433fd64b08d75d8f4031', N'+01a2goX1ka2V+Ma2hepDw==', N'814d894305f849dc9e55ce1273ac8b21', N'94c045a17327e0caf938b486b7f3cd4b', N'434bb975fde1433fd64b08d75d8f4031', N'2017-02-21 10:32:17.913')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'958d74c2336101138769df0902657dd499be8c17f6b921a2e8d38ad186a16857', N'Wxe/Z/hnsUie0WX+AEUTEw==', N'2a2f7d83a3314b9a84047900a32112d4', N'958d74c2336101138769df0902657dd4', N'99be8c17f6b921a2e8d38ad186a16857', N'2017-03-15 11:45:11.587')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'9640871747b653afe12b9bfd9d8ea9137e78299cd9b591a8ffffc88c3cfceba7', N'bLqkasLZk0eqAEnWQetJoA==', N'47daa1961a6642a4b8ed9f6b43647d2e', N'9640871747b653afe12b9bfd9d8ea913', N'7e78299cd9b591a8ffffc88c3cfceba7', N'2017-03-14 10:14:19.903')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'987ce8e9cda537799301b30625d45f5b9d3d724abe54fded81d4b141b847359d', N'3uiL9dZzrESr//7Yk7IIPQ==', N'645dcfa227974be0b993234938926d72', N'987ce8e9cda537799301b30625d45f5b', N'9d3d724abe54fded81d4b141b847359d', N'2016-12-30 18:54:06.713')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'9890da7475e9f954d8dd1392d0818b1acfa002c8a3987ec21fb23f6e79ae558e', N'Oyg/JkYYT0uJRA+JhCkfOw==', N'5c4f46ad811b49df8f186f8c9bd1c76e', N'9890da7475e9f954d8dd1392d0818b1a', N'cfa002c8a3987ec21fb23f6e79ae558e', N'2017-03-14 12:04:07.360')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'98a08f8332f54a659526cfae498855b88a56d860d2752d6f8c29bc10110fa343', N'9Y5VmhrFZkCwYCqQ3jTNpw==', N'201099513187426e84c6213b4169ba31', N'98a08f8332f54a659526cfae498855b8', N'8a56d860d2752d6f8c29bc10110fa343', N'2017-01-17 16:12:18.770')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'9a5408412f74a0f54b41d6556cf9acbe41b5876ae57d3c04a0f45684dd9e670f', N'aL3Fxeevr0uOuf1JdbeIoA==', N'7aa1606e833643058c33f14353e74a38', N'9a5408412f74a0f54b41d6556cf9acbe', N'41b5876ae57d3c04a0f45684dd9e670f', N'2017-02-15 14:28:56.567')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'9c05168f0cb4f77d803cc777119d0cf2f502c34b35740dd42201f29b8ca544cd', N'Xc8SLv5aok6JMXU9F8VSpg==', N'0f6a2d77ffac40129117f78ce978c1cb', N'9c05168f0cb4f77d803cc777119d0cf2', N'f502c34b35740dd42201f29b8ca544cd', N'2017-02-09 11:46:38.487')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'9ca0bf143428682f6d6d92879046927f948b0ac6f6bc812f510fd2b9c460a9b2', N'DTKoijnsGUeRVIbN35z7BA==', N'a7cd4548da0142cb9e7fca20e8b01827', N'9ca0bf143428682f6d6d92879046927f', N'948b0ac6f6bc812f510fd2b9c460a9b2', N'2017-02-28 09:37:56.977')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'9e5ad9edc739f01e67c8a80975da180ed68450258fe3461551613ba7e1f4d552', N'3N7z92PcLUmuVgnvyQ//tQ==', N'a1926ff9277a4419924305492da592d0', N'9e5ad9edc739f01e67c8a80975da180e', N'd68450258fe3461551613ba7e1f4d552', N'2017-02-20 18:43:43.007')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'9ec64c9f3cf2f097f70dd52b17184b5f3261d6c7b59e4e479b2e1beb613750fe', N'rQAsqUF8hkOp/mQhGXGfWQ==', N'8e6566c2728643b99116d40a6780f69e', N'9ec64c9f3cf2f097f70dd52b17184b5f', N'3261d6c7b59e4e479b2e1beb613750fe', N'2017-01-17 17:32:32.273')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'9ecd7e7cb669460eb197687d0ac82641ccd6620dd2aab33c10461115fe124115', N'JvEgNm3aCkiPBkBD8fjBzQ==', N'2a559f17c6994063b22df6ab4c8212d1', N'9ecd7e7cb669460eb197687d0ac82641', N'ccd6620dd2aab33c10461115fe124115', N'2017-01-20 14:44:52.380')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'9ece608edad2f22e25d860b0ef5cf78e49d87e80cdc6e59287a473ae5ea62170', N'TMg3cwJjt06xnNiQ7oYvAA==', N'22f63f7476db43be88b0be8bc9966402', N'9ece608edad2f22e25d860b0ef5cf78e', N'49d87e80cdc6e59287a473ae5ea62170', N'2017-01-16 20:15:46.227')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'9f1f8f3550e6ff347f930b9cca961b60b3395c98ae6987ca70a651753fa880e9', N'8X5K4zg0gkugM3++YV87BA==', N'6cf6ceb287ce4ee9bd992c5b75148efa', N'9f1f8f3550e6ff347f930b9cca961b60', N'b3395c98ae6987ca70a651753fa880e9', N'2017-02-06 17:09:26.857')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'9f416297554347367a3bb01bcbd9a90820225533df4ce9d0497ad47aad12d824', N'dW5Z7Cyzc0uLPv5p9ooyyg==', N'ebd5d879a82f4df8832ef0d1b23666c7', N'9f416297554347367a3bb01bcbd9a908', N'20225533df4ce9d0497ad47aad12d824', N'2017-03-17 18:24:12.173')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'9f6d45ae087e58ca2ec6a73a62cde902d42c5aae4facd4761685eee12ee775d3', N'kXl7cqEP9UaCHPMLinV3Ng==', N'38614b851854498699aafd00206d1581', N'9f6d45ae087e58ca2ec6a73a62cde902', N'd42c5aae4facd4761685eee12ee775d3', N'2016-12-21 19:31:58.217')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'a062beb09b91cfff2408153fbf2c52cb434dabfcac609fdf1530ba50154f6441', N'1diTeBY9nk64kEAQswfJFg==', N'8558e2e9ea214ce59b692777c359b51c', N'a062beb09b91cfff2408153fbf2c52cb', N'434dabfcac609fdf1530ba50154f6441', N'2017-03-20 21:02:41.057')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'a08c339c745d7137abf371912a3024c58a56d860d2752d6f8c29bc10110fa343', N'WHC7wbF99EaQf7VO4uYJFw==', N'201099513187426e84c6213b4169ba31', N'a08c339c745d7137abf371912a3024c5', N'8a56d860d2752d6f8c29bc10110fa343', N'2017-02-22 16:37:17.113')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'a0ce243cf8bd1260b8e684a284be0dc88a56d860d2752d6f8c29bc10110fa343', N'kUzByfNZ+0uV8oHvDc4ZKw==', N'201099513187426e84c6213b4169ba31', N'a0ce243cf8bd1260b8e684a284be0dc8', N'8a56d860d2752d6f8c29bc10110fa343', N'2017-01-17 12:11:21.610')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'a0f14bd56755a400c37845a4630bd9af58a044727b3b4b5ef6c2922ffeaded87', N'Mw/1RSIVR0GJjVGunDOukg==', N'0aabd833542f4319b5e96dfff605f8e8', N'a0f14bd56755a400c37845a4630bd9af', N'58a044727b3b4b5ef6c2922ffeaded87', N'2016-12-27 10:14:55.173')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'a213b1a2557f77ba0148b814fb70545301e656bf4b605f0f35d2cfa441856574', N'1UzzuUvkAUS2NLxzc2vXyw==', N'58f2655c3a1c40d09bb9d86eafa03b20', N'a213b1a2557f77ba0148b814fb705453', N'01e656bf4b605f0f35d2cfa441856574', N'2017-03-15 15:04:57.290')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'a263046600ebbac8c162dc8b9e04f6e1207ec085fe40d6523afd9b7c26cb66e6', N'N/7XqiJCRE6MNq40D+tR8Q==', N'6969a38a50c34339824ff806b77897fa', N'a263046600ebbac8c162dc8b9e04f6e1', N'207ec085fe40d6523afd9b7c26cb66e6', N'2017-03-22 18:27:30.170')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'a2fa71c4b90a1f49f44d762e24e58ced473a784208c2304a2235e57899a5817f', N'MWaa6pdetUWKR76cq+1tBw==', N'cbec89a064f04cadbad7f7924079c397', N'a2fa71c4b90a1f49f44d762e24e58ced', N'473a784208c2304a2235e57899a5817f', N'2016-12-27 10:11:47.223')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'a45598c482308f2fdd22931b30efa3843ac7b6a2bc978654d2832e3b8646bb78', N'jURuGuew90qC0aqpjJLnBQ==', N'1c4be1bd9b4c4aa8a5627ca6d97ba27a', N'a45598c482308f2fdd22931b30efa384', N'3ac7b6a2bc978654d2832e3b8646bb78', N'2017-03-27 15:44:52.173')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'a56380740d75b8180d1a1eb13c5b7d30de15d7fd90e3fe988e12cf6c4a7976ba', N'Oz2zc1wTuU+ZV3YJDcMnUg==', N'7d92d0ec2e9144d3bd3047df270adac0', N'a56380740d75b8180d1a1eb13c5b7d30', N'de15d7fd90e3fe988e12cf6c4a7976ba', N'2017-02-13 17:10:52.767')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'a5acf138422e192ef674dabb35d2452307bcd6d046565d27b853fa5a5cf07de6', N'LFmqr410RUq4v7iQLG2PxA==', N'dcb8775889f34281b75e97ce474be599', N'a5acf138422e192ef674dabb35d24523', N'07bcd6d046565d27b853fa5a5cf07de6', N'2017-02-18 20:50:03.550')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'a645498723753b1fce7bdc86eedf1c98c38b61358a1b99011ba61c7088b2770c', N'vF45/UJeb0ubN0b3Q0gW0Q==', N'2fdc5b56e9b342a79b068beb282c89b4', N'a645498723753b1fce7bdc86eedf1c98', N'c38b61358a1b99011ba61c7088b2770c', N'2016-12-28 20:13:33.917')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'a6468f7cd022c7b4c2a617ff32e7748d9d5f0122a12708c1c2b5b298a22fe7fe', N'QFj35tLdxECsvETfLHhAPw==', N'd499eda1d9ae45649294ea157467e9b3', N'a6468f7cd022c7b4c2a617ff32e7748d', N'9d5f0122a12708c1c2b5b298a22fe7fe', N'2017-01-17 16:16:36.660')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'a733ea6114627683583cd98767be30550d4308399091742d66d3b9c9e7392542', N'5s90C4IczEiBprVJyZ2tCQ==', N'a89940dc98694863b05b5d6c5fb7a894', N'a733ea6114627683583cd98767be3055', N'0d4308399091742d66d3b9c9e7392542', N'2017-03-15 14:59:09.893')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'a7628da769f3ec6e6ccf75be4d4f13c739772ec635665f1acfb50101cfc29b88', N'kGcwkVvoK06wWlnd4uVH/Q==', N'6cee04da93804555a23417b379de1b98', N'a7628da769f3ec6e6ccf75be4d4f13c7', N'39772ec635665f1acfb50101cfc29b88', N'2017-01-03 17:32:19.420')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'a7ddce515f3edc5e140f53f6b0085a38a454807dbfbfd000e779aabbf1268411', N'r2hA4hc0eU+861x5/pItvw==', N'c20d52070264491f9c09868bb11de0c3', N'a7ddce515f3edc5e140f53f6b0085a38', N'a454807dbfbfd000e779aabbf1268411', N'2017-03-28 12:19:30.527')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'a7e3f903a871f4af56e549a201a42060a67aad03c40a6dddd031237fb02a141d', N'Uu6NyTwGFkmFe0x4/lWN5g==', N'1f9405fe7d0548fab1acd6da61bb4b10', N'a7e3f903a871f4af56e549a201a42060', N'a67aad03c40a6dddd031237fb02a141d', N'2016-12-27 12:10:29.360')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'a8245216d52b832b4685d2fb5e64c774ea1dd529ce2ffce52e18b43792988c6c', N'z/z7yhZOskq0tFDl7SBm7w==', N'3bcd190b5683461583b1a877137a4653', N'a8245216d52b832b4685d2fb5e64c774', N'ea1dd529ce2ffce52e18b43792988c6c', N'2017-03-14 10:56:13.090')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'a86ac5cf501cdcabbf0624c756810106a0c85a1aef260e1c969063756f097093', N'Xw9xePsY0kynRjxBP3R6zw==', N'fc637043ff1249bda903ce0e1849e814', N'a86ac5cf501cdcabbf0624c756810106', N'a0c85a1aef260e1c969063756f097093', N'2017-03-17 10:53:14.587')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'a86c7e88efd5a8de4ab61d2d9f03ebc03be87bd87497699691180ca32550d339', N'HlgfdpeeekWmK1+umYUyWw==', N'5f37b51f285448d79880ccd836145fea', N'a86c7e88efd5a8de4ab61d2d9f03ebc0', N'3be87bd87497699691180ca32550d339', N'2017-01-20 15:18:13.393')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'a8a465757af1db4b192bae7eb72b48b2c33027d222b82a884631ae0aef273fb1', N'22TMHoC1LkKPr+Bn5qVuQg==', N'f07b994cb61e4accb52d1ed16db222dc', N'a8a465757af1db4b192bae7eb72b48b2', N'c33027d222b82a884631ae0aef273fb1', N'2017-02-07 18:38:44.627')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'aa9244ec196b037b74e22e52af44d3ed0222846f0c1a698884d3ca8c063c6541', N'editer_token', N'3ca5736951994381a4334af72e795084', N'aa9244ec196b037b74e22e52af44d3ed', N'0222846f0c1a698884d3ca8c063c6541', N'2017-02-04 10:54:37.970')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'aa9244ec196b037b74e22e52af44d3ed02630cfbe694b5da0ed4715c3b96788d', N'editer_token', N'dcff89f42ee24b8882cdcc2f25fc3de0', N'aa9244ec196b037b74e22e52af44d3ed', N'02630cfbe694b5da0ed4715c3b96788d', N'2017-03-21 15:09:23.987')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'aa9244ec196b037b74e22e52af44d3ed02f72873f6d8ba369988a3f93b553842', N'editer_token', N'631c061dfbac41b4b85c4dac16c5221a', N'aa9244ec196b037b74e22e52af44d3ed', N'02f72873f6d8ba369988a3f93b553842', N'2017-02-18 16:33:03.173')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'aa9244ec196b037b74e22e52af44d3ed036afe166f31f029150900192dd06b53', N'editer_token', N'9e4cadd542084b43820096b745579737', N'aa9244ec196b037b74e22e52af44d3ed', N'036afe166f31f029150900192dd06b53', N'2016-12-26 09:38:03.907')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'aa9244ec196b037b74e22e52af44d3ed0506e7d2bffa7cfeb340c7a2a298c269', N'editer_token', N'8ce11ea0dc354c908de13d04b173f9ec', N'aa9244ec196b037b74e22e52af44d3ed', N'0506e7d2bffa7cfeb340c7a2a298c269', N'2016-12-26 20:26:02.690')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'aa9244ec196b037b74e22e52af44d3ed0721fc1dc822831b8913914132b70ab7', N'editer_token', N'704bcaf708154ab9be9ede425b09a598', N'aa9244ec196b037b74e22e52af44d3ed', N'0721fc1dc822831b8913914132b70ab7', N'2016-12-12 17:39:36.650')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'aa9244ec196b037b74e22e52af44d3ed083e9fc89449ccf57fd21bd5a5209ba9', N'editer_token', N'321932b2f11241e69f60e3edd1938b41', N'aa9244ec196b037b74e22e52af44d3ed', N'083e9fc89449ccf57fd21bd5a5209ba9', N'2017-02-25 15:32:52.820')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'aa9244ec196b037b74e22e52af44d3ed08b3fa546b43753601251d1bec08fe41', N'editer_token', N'3a1c65153ec84a5f85f27a58bfd8deb6', N'aa9244ec196b037b74e22e52af44d3ed', N'08b3fa546b43753601251d1bec08fe41', N'2016-12-23 10:57:35.393')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'aa9244ec196b037b74e22e52af44d3ed08ed444df976976a01822aebb9cd5d3f', N'editer_token', N'0458205228ef4213bfdefae7463c6898', N'aa9244ec196b037b74e22e52af44d3ed', N'08ed444df976976a01822aebb9cd5d3f', N'2016-12-27 10:14:43.353')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'aa9244ec196b037b74e22e52af44d3ed0965085e34121c7e546fbf73fa945cab', N'editer_token', N'21e5d2ca399742adbb18606d5a77ebc0', N'aa9244ec196b037b74e22e52af44d3ed', N'0965085e34121c7e546fbf73fa945cab', N'2016-12-29 09:43:01.493')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'aa9244ec196b037b74e22e52af44d3ed0a6fe463920a29ce9feae568cac667da', N'editer_token', N'b70405c6ba59411484493c406438aff9', N'aa9244ec196b037b74e22e52af44d3ed', N'0a6fe463920a29ce9feae568cac667da', N'2016-12-12 19:41:12.483')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'aa9244ec196b037b74e22e52af44d3ed0ab7c8a80f3422bd5b04fbd2c6bd8d58', N'editer_token', N'be8f5d6396bd4d7fb69d4f325e287217', N'aa9244ec196b037b74e22e52af44d3ed', N'0ab7c8a80f3422bd5b04fbd2c6bd8d58', N'2017-01-19 20:26:52.160')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'aa9244ec196b037b74e22e52af44d3ed0bd28d6c079c88f208540441892d870f', N'editer_token', N'31ff7da486bb42d1b2c98331d44d3172', N'aa9244ec196b037b74e22e52af44d3ed', N'0bd28d6c079c88f208540441892d870f', N'2017-03-22 12:26:33.560')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'aa9244ec196b037b74e22e52af44d3ed0f8a3887b81ea8848bb492a3302a486e', N'editer_token', N'0fddd42b27b64ddfa9f853d7dd70a0a3', N'aa9244ec196b037b74e22e52af44d3ed', N'0f8a3887b81ea8848bb492a3302a486e', N'2016-12-20 10:25:04.563')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'aa9244ec196b037b74e22e52af44d3ed1096d3efce7ef3ada4ecc1bc86331d4f', N'editer_token', N'8444c317f8fc4d00bf1e1ff2f737bc1c', N'aa9244ec196b037b74e22e52af44d3ed', N'1096d3efce7ef3ada4ecc1bc86331d4f', N'2016-12-26 19:52:00.090')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'aa9244ec196b037b74e22e52af44d3ed143047ab7723a880e424d29c2924ffa0', N'editer_token', N'c0e6217f8fb043d7b886438d6e53e96a', N'aa9244ec196b037b74e22e52af44d3ed', N'143047ab7723a880e424d29c2924ffa0', N'2017-02-15 17:03:37.830')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'aa9244ec196b037b74e22e52af44d3ed16db00adecf8132b6afc57ad4fe8469c', N'editer_token', N'a86de24e54254af38a54e46e0850b9f9', N'aa9244ec196b037b74e22e52af44d3ed', N'16db00adecf8132b6afc57ad4fe8469c', N'2016-12-14 16:16:21.423')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'aa9244ec196b037b74e22e52af44d3ed177d7f3e4f863b5883b9853fa90df253', N'editer_token', N'8d643be6c54a42aba577c60da7809b54', N'aa9244ec196b037b74e22e52af44d3ed', N'177d7f3e4f863b5883b9853fa90df253', N'2017-01-17 16:26:57.063')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'aa9244ec196b037b74e22e52af44d3ed198e9145a3abdc7f62de1ee4b1e6c0d7', N'editer_token', N'9cf75103d02f4a5690ef17e6bcd3bee7', N'aa9244ec196b037b74e22e52af44d3ed', N'198e9145a3abdc7f62de1ee4b1e6c0d7', N'2016-12-27 10:13:42.367')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'aa9244ec196b037b74e22e52af44d3ed19a6b75ac871b626ab19754d7fb9f33a', N'editer_token', N'a22ae728cf60444da139874b5680fa77', N'aa9244ec196b037b74e22e52af44d3ed', N'19a6b75ac871b626ab19754d7fb9f33a', N'2017-03-21 18:46:17.457')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'aa9244ec196b037b74e22e52af44d3ed1f8bc42733b81b5584ee4b33bd748153', N'editer_token', N'de95b524a4f54016a3feeee3b113bfbb', N'aa9244ec196b037b74e22e52af44d3ed', N'1f8bc42733b81b5584ee4b33bd748153', N'2017-02-21 18:20:22.970')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'aa9244ec196b037b74e22e52af44d3ed218fd359b5ff8e2dad6da515403facef', N'editer_token', N'47deb39e95fc47ebadb3540049627750', N'aa9244ec196b037b74e22e52af44d3ed', N'218fd359b5ff8e2dad6da515403facef', N'2016-11-28 19:10:14.420')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'aa9244ec196b037b74e22e52af44d3ed23ffbd4c9bb89477c86374b7e481025b', N'editer_token', N'fe162707736346daae771351adbf66c0', N'aa9244ec196b037b74e22e52af44d3ed', N'23ffbd4c9bb89477c86374b7e481025b', N'2016-12-27 10:13:46.883')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'aa9244ec196b037b74e22e52af44d3ed24f7187b98719356756814b2c0060510', N'editer_token', N'0e41449ca2c74d9e8de4bcaf8ac9600b', N'aa9244ec196b037b74e22e52af44d3ed', N'24f7187b98719356756814b2c0060510', N'2017-02-16 19:33:16.983')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'aa9244ec196b037b74e22e52af44d3ed25116ca661636fe89b42372cb669832e', N'editer_token', N'ad90391791be41989ecef11f122e6af8', N'aa9244ec196b037b74e22e52af44d3ed', N'25116ca661636fe89b42372cb669832e', N'2017-02-22 10:55:33.083')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'aa9244ec196b037b74e22e52af44d3ed27fab9be70973393c735352db77c66b6', N'editer_token', N'2f75bab1966943d59deaa3de8c89895a', N'aa9244ec196b037b74e22e52af44d3ed', N'27fab9be70973393c735352db77c66b6', N'2016-11-28 19:18:19.517')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'aa9244ec196b037b74e22e52af44d3ed2a1057b1cff369d33dbd2613830cdec0', N'editer_token', N'3ab240fba7a34360abaac415767c167a', N'aa9244ec196b037b74e22e52af44d3ed', N'2a1057b1cff369d33dbd2613830cdec0', N'2017-02-06 14:41:44.727')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'aa9244ec196b037b74e22e52af44d3ed2ba8303c892d03a924904389edccaa40', N'editer_token', N'2a7b28b33d7f48ac9f3650d5de0f3cd6', N'aa9244ec196b037b74e22e52af44d3ed', N'2ba8303c892d03a924904389edccaa40', N'2017-03-20 13:49:52.567')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'aa9244ec196b037b74e22e52af44d3ed2ccccc7a5bf9c73f05d7afe8512f39d1', N'editer_token', N'38eabd4a8a9f4f8481cdf08b1e8baf64', N'aa9244ec196b037b74e22e52af44d3ed', N'2ccccc7a5bf9c73f05d7afe8512f39d1', N'2016-12-27 10:13:39.993')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'aa9244ec196b037b74e22e52af44d3ed2fd6aca34317366897e8d17c6f86fc5d', N'editer_token', N'077c4fad3daf41d69a4e49f0b8ba89dc', N'aa9244ec196b037b74e22e52af44d3ed', N'2fd6aca34317366897e8d17c6f86fc5d', N'2016-12-28 19:55:52.033')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'aa9244ec196b037b74e22e52af44d3ed31325f79ce34d4f1a58d8cd0262586a0', N'editer_token', N'6e5d56dbc7b94279af72fdc2dfbc74ce', N'aa9244ec196b037b74e22e52af44d3ed', N'31325f79ce34d4f1a58d8cd0262586a0', N'2017-01-22 17:15:36.423')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'aa9244ec196b037b74e22e52af44d3ed334ede9b93b56f99ee4ad5e3a8a74465', N'editer_token', N'8252906fadb448a5b62e2fe5248e56a4', N'aa9244ec196b037b74e22e52af44d3ed', N'334ede9b93b56f99ee4ad5e3a8a74465', N'2016-12-01 09:51:50.467')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'aa9244ec196b037b74e22e52af44d3ed35506930a69fe694f225fcefa9ce98e5', N'editer_token', N'b6cfcec71b0e4a8ca8acb35bad020694', N'aa9244ec196b037b74e22e52af44d3ed', N'35506930a69fe694f225fcefa9ce98e5', N'2017-03-20 13:47:56.883')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'aa9244ec196b037b74e22e52af44d3ed38f6faf14512d1ddcca2cc71ef09f41d', N'editer_token', N'345832ebf50f4f9ebdcb268d99f7928e', N'aa9244ec196b037b74e22e52af44d3ed', N'38f6faf14512d1ddcca2cc71ef09f41d', N'2016-12-22 14:54:08.913')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'aa9244ec196b037b74e22e52af44d3ed394521bcffcaef8930452fb7636ec18c', N'editer_token', N'96b6968d879945538bddafaf66d0090e', N'aa9244ec196b037b74e22e52af44d3ed', N'394521bcffcaef8930452fb7636ec18c', N'2017-02-15 13:51:52.620')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'aa9244ec196b037b74e22e52af44d3ed394b1d6c40f714886483b1994c3ccf2e', N'editer_token', N'8764b0fb93c44a03a29e695748001c56', N'aa9244ec196b037b74e22e52af44d3ed', N'394b1d6c40f714886483b1994c3ccf2e', N'2017-03-04 19:03:11.377')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'aa9244ec196b037b74e22e52af44d3ed3962b11869a1e47a3c22a3360fd3d8bf', N'editer_token', N'9a6366d5bccd46d68eadd4c0bec51923', N'aa9244ec196b037b74e22e52af44d3ed', N'3962b11869a1e47a3c22a3360fd3d8bf', N'2016-12-29 17:18:30.817')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'aa9244ec196b037b74e22e52af44d3ed3a8889ab0dba8562ad8e1789b27531a7', N'editer_token', N'92c54048ff304bf7b5b9ca95d74847d4', N'aa9244ec196b037b74e22e52af44d3ed', N'3a8889ab0dba8562ad8e1789b27531a7', N'2016-12-22 16:03:26.247')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'aa9244ec196b037b74e22e52af44d3ed3d384f742a87b84f0db638c87027d74e', N'editer_token', N'2f8207f9901f410a857627a052949b30', N'aa9244ec196b037b74e22e52af44d3ed', N'3d384f742a87b84f0db638c87027d74e', N'2017-02-09 16:08:10.490')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'aa9244ec196b037b74e22e52af44d3ed3e92786455d1bc1f0581eec6e719985e', N'editer_token', N'44891dbeb271462fafac1262873042d5', N'aa9244ec196b037b74e22e52af44d3ed', N'3e92786455d1bc1f0581eec6e719985e', N'2016-12-22 12:59:20.433')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'aa9244ec196b037b74e22e52af44d3ed3edc50dd2396921a908931289d71f28b', N'editer_token', N'db5ffea773984ebfa38e426000ba2762', N'aa9244ec196b037b74e22e52af44d3ed', N'3edc50dd2396921a908931289d71f28b', N'2017-01-22 22:19:23.040')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'aa9244ec196b037b74e22e52af44d3ed3f4c155918ca3e2d348dbdefe346978e', N'editer_token', N'aede9414a73745139f3cc44cdae74961', N'aa9244ec196b037b74e22e52af44d3ed', N'3f4c155918ca3e2d348dbdefe346978e', N'2016-12-26 15:56:57.670')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'aa9244ec196b037b74e22e52af44d3ed427b712a58d78c72af2ab726e535c365', N'editer_token', N'af51510fc71e461887a052f2793ffe31', N'aa9244ec196b037b74e22e52af44d3ed', N'427b712a58d78c72af2ab726e535c365', N'2016-12-08 14:43:11.267')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'aa9244ec196b037b74e22e52af44d3ed468243161a54c7849067ad556f722e1d', N'editer_token', N'efec5f3cd07f47b88c874ea084e93d7f', N'aa9244ec196b037b74e22e52af44d3ed', N'468243161a54c7849067ad556f722e1d', N'2016-12-23 17:02:26.800')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'aa9244ec196b037b74e22e52af44d3ed47343a4c5cc45fbe949371943e7efccb', N'editer_token', N'8feff70c792a405192332b7fd446210d', N'aa9244ec196b037b74e22e52af44d3ed', N'47343a4c5cc45fbe949371943e7efccb', N'2017-01-16 20:15:16.540')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'aa9244ec196b037b74e22e52af44d3ed49e468cf25bbe169c866f0415ae79778', N'editer_token', N'c8430585ea8a4c5a83e351c2590a83e9', N'aa9244ec196b037b74e22e52af44d3ed', N'49e468cf25bbe169c866f0415ae79778', N'2016-12-08 14:11:05.457')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'aa9244ec196b037b74e22e52af44d3ed4b92b4ce741ef506e67e18da130c83b6', N'editer_token', N'c2843513ca494ef3bc5ae617197370e5', N'aa9244ec196b037b74e22e52af44d3ed', N'4b92b4ce741ef506e67e18da130c83b6', N'2016-12-27 10:15:33.947')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'aa9244ec196b037b74e22e52af44d3ed4bb89d79f7fb4c9e7889745467dcf7f6', N'editer_token', N'ec3b64b819c04de4a1b697f71d3e466c', N'aa9244ec196b037b74e22e52af44d3ed', N'4bb89d79f7fb4c9e7889745467dcf7f6', N'2016-12-01 16:09:39.070')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'aa9244ec196b037b74e22e52af44d3ed4c2153c9c84fa216d406985c2fc1b7e6', N'editer_token', N'edf02bd53e444f10a60a2d4355508f4d', N'aa9244ec196b037b74e22e52af44d3ed', N'4c2153c9c84fa216d406985c2fc1b7e6', N'2017-02-15 16:42:39.027')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'aa9244ec196b037b74e22e52af44d3ed4c4d7674fc13fe76baa1b12c223eab6c', N'editer_token', N'4d9b62fe0b0e4c5ca95adb29a37e69c6', N'aa9244ec196b037b74e22e52af44d3ed', N'4c4d7674fc13fe76baa1b12c223eab6c', N'2017-03-23 10:32:02.643')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'aa9244ec196b037b74e22e52af44d3ed4c5370bf469516914fff695993bba90a', N'editer_token', N'd45fac41f05148faafbe31dd1f637077', N'aa9244ec196b037b74e22e52af44d3ed', N'4c5370bf469516914fff695993bba90a', N'2016-12-22 12:35:50.983')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'aa9244ec196b037b74e22e52af44d3ed4f2e86fdd7b6573f713e8d0600f93d70', N'editer_token', N'238872f9eb454487b6650a67b9e36e1f', N'aa9244ec196b037b74e22e52af44d3ed', N'4f2e86fdd7b6573f713e8d0600f93d70', N'2017-01-23 17:57:06.550')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'aa9244ec196b037b74e22e52af44d3ed4fceae147267dcbdf9d0c846f2e60893', N'editer_token', N'5321c2cb9ef44287b762bac559522668', N'aa9244ec196b037b74e22e52af44d3ed', N'4fceae147267dcbdf9d0c846f2e60893', N'2016-12-03 16:09:44.400')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'aa9244ec196b037b74e22e52af44d3ed5345b52abb122e818f1323f3cbd3597b', N'editer_token', N'c2fcc0f63efb47ab954f7fc3a4c78498', N'aa9244ec196b037b74e22e52af44d3ed', N'5345b52abb122e818f1323f3cbd3597b', N'2017-01-20 18:54:08.000')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'aa9244ec196b037b74e22e52af44d3ed54f39594ed27ded19449e7db5df9072d', N'editer_token', N'52c4b5b962024c8084e3df19e4751a76', N'aa9244ec196b037b74e22e52af44d3ed', N'54f39594ed27ded19449e7db5df9072d', N'2016-12-01 16:41:46.337')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'aa9244ec196b037b74e22e52af44d3ed554dc8038fdf3d6f02be4cc81f38144e', N'editer_token', N'c3c6993b7245490b8acec7209018f1eb', N'aa9244ec196b037b74e22e52af44d3ed', N'554dc8038fdf3d6f02be4cc81f38144e', N'2016-11-25 13:50:55.717')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'aa9244ec196b037b74e22e52af44d3ed5ac52b8751a864fd8288d0f2b1cfd481', N'editer_token', N'f126b48c239b4e7785989602dff93689', N'aa9244ec196b037b74e22e52af44d3ed', N'5ac52b8751a864fd8288d0f2b1cfd481', N'2016-12-15 11:16:24.603')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'aa9244ec196b037b74e22e52af44d3ed5e8e01a8720f5a4db5fcad3a12026f54', N'editer_token', N'175b8fae60744665a6d105306f13c19c', N'aa9244ec196b037b74e22e52af44d3ed', N'5e8e01a8720f5a4db5fcad3a12026f54', N'2016-12-07 11:10:00.697')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'aa9244ec196b037b74e22e52af44d3ed615efb07b30bcbc3e70ffb24ff9684cd', N'editer_token', N'7226e44ed4ff439f8cee9ea8844c4887', N'aa9244ec196b037b74e22e52af44d3ed', N'615efb07b30bcbc3e70ffb24ff9684cd', N'2017-01-24 16:00:21.770')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'aa9244ec196b037b74e22e52af44d3ed61808bb76f82a41803057f616e164ed9', N'editer_token', N'c92ce3a9fcce4a6785e176bbef772c4c', N'aa9244ec196b037b74e22e52af44d3ed', N'61808bb76f82a41803057f616e164ed9', N'2017-01-25 14:17:26.210')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'aa9244ec196b037b74e22e52af44d3ed6270865119519afbba81e8183c5bdf2f', N'editer_token', N'efbfcfd0e10c4f60a37cb355119e628e', N'aa9244ec196b037b74e22e52af44d3ed', N'6270865119519afbba81e8183c5bdf2f', N'2016-12-16 16:01:09.037')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'aa9244ec196b037b74e22e52af44d3ed65c7ae8772755f5c06a430a493aeb58f', N'editer_token', N'312263808cf94adaa8ca0c955b7cf815', N'aa9244ec196b037b74e22e52af44d3ed', N'65c7ae8772755f5c06a430a493aeb58f', N'2017-02-09 16:22:17.263')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'aa9244ec196b037b74e22e52af44d3ed6a08ab2668e112326a6f3e46ad63609b', N'editer_token', N'02c81e85662c45e593a6800a46be32f7', N'aa9244ec196b037b74e22e52af44d3ed', N'6a08ab2668e112326a6f3e46ad63609b', N'2017-03-21 15:07:10.007')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'aa9244ec196b037b74e22e52af44d3ed71dd969a2392087a6cd447c2eaf40b01', N'editer_token', N'ae4adc23879e4ee0bd700225c410e813', N'aa9244ec196b037b74e22e52af44d3ed', N'71dd969a2392087a6cd447c2eaf40b01', N'2017-03-23 16:59:01.643')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'aa9244ec196b037b74e22e52af44d3ed71e46d093b05b0b92cc91032ef98ee4e', N'editer_token', N'5f38a71b4b344dc3be4a2bab71123925', N'aa9244ec196b037b74e22e52af44d3ed', N'71e46d093b05b0b92cc91032ef98ee4e', N'2017-02-06 17:14:53.023')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'aa9244ec196b037b74e22e52af44d3ed746389d042792b033f82d5afadcc8a29', N'editer_token', N'42accd05c87647b2b3784c598bb60c20', N'aa9244ec196b037b74e22e52af44d3ed', N'746389d042792b033f82d5afadcc8a29', N'2017-01-23 15:50:34.367')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'aa9244ec196b037b74e22e52af44d3ed75c0bfe7608eba5d36131ecbb48c438f', N'editer_token', N'567b7a6fc6cf4631a3b68b2b39a0b0bc', N'aa9244ec196b037b74e22e52af44d3ed', N'75c0bfe7608eba5d36131ecbb48c438f', N'2017-03-28 16:51:46.843')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'aa9244ec196b037b74e22e52af44d3ed76a74096781e1b0f46d503409a754a7a', N'editer_token', N'd553cc0930c34e968be6472796ed340c', N'aa9244ec196b037b74e22e52af44d3ed', N'76a74096781e1b0f46d503409a754a7a', N'2017-01-22 23:18:35.160')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'aa9244ec196b037b74e22e52af44d3ed7930a7dbb2b9b17049fb800cd5495a8d', N'editer_token', N'4acad56f6e3f4e4cae952f5ff6188fea', N'aa9244ec196b037b74e22e52af44d3ed', N'7930a7dbb2b9b17049fb800cd5495a8d', N'2016-12-22 11:40:32.440')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'aa9244ec196b037b74e22e52af44d3ed7b9c513511f4e00912e9abad2aae5d83', N'editer_token', N'335b646799224fa782714c2fffd3149a', N'aa9244ec196b037b74e22e52af44d3ed', N'7b9c513511f4e00912e9abad2aae5d83', N'2017-03-28 15:49:32.007')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'aa9244ec196b037b74e22e52af44d3ed7c5e805e485a5d2c9661b7c4947db10d', N'editer_token', N'a3102ee319e44cd88632822fe2ab729c', N'aa9244ec196b037b74e22e52af44d3ed', N'7c5e805e485a5d2c9661b7c4947db10d', N'2017-02-09 16:11:00.500')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'aa9244ec196b037b74e22e52af44d3ed7cfa7513ef59d69ca634d124e4c42606', N'editer_token', N'97a695bc7d26459e97408b0442d8b82a', N'aa9244ec196b037b74e22e52af44d3ed', N'7cfa7513ef59d69ca634d124e4c42606', N'2017-03-21 16:55:09.940')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'aa9244ec196b037b74e22e52af44d3ed7e5f548ad025c5b0b0b55a3232d8d72d', N'editer_token', N'71a32997af4a4eb895f41ea1fe769ab0', N'aa9244ec196b037b74e22e52af44d3ed', N'7e5f548ad025c5b0b0b55a3232d8d72d', N'2017-03-24 18:42:43.087')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'aa9244ec196b037b74e22e52af44d3ed7eb30774b06b527d7b4dd60d4b9f3e7a', N'editer_token', N'd8712cb1d25f484f9f34e5bb15d7ffa0', N'aa9244ec196b037b74e22e52af44d3ed', N'7eb30774b06b527d7b4dd60d4b9f3e7a', N'2017-02-08 16:12:40.577')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'aa9244ec196b037b74e22e52af44d3ed7f439dec4f4c34f7df7a942dc85ed406', N'editer_token', N'8853d83f460b4d8eb997caabdadd0f19', N'aa9244ec196b037b74e22e52af44d3ed', N'7f439dec4f4c34f7df7a942dc85ed406', N'2017-01-22 17:46:32.720')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'aa9244ec196b037b74e22e52af44d3ed83442a889175e204672bc070e41ba26a', N'editer_token', N'2e15d4a369ad46a3b3c1b56988bc7dca', N'aa9244ec196b037b74e22e52af44d3ed', N'83442a889175e204672bc070e41ba26a', N'2017-01-22 17:02:58.757')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'aa9244ec196b037b74e22e52af44d3ed863128808120f3dab58723be09adfb08', N'editer_token', N'e68ac1be6ddb430e8fbbb4de9420f499', N'aa9244ec196b037b74e22e52af44d3ed', N'863128808120f3dab58723be09adfb08', N'2017-01-23 16:52:37.210')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'aa9244ec196b037b74e22e52af44d3ed8a783040f0a843f63b708db14832eeeb', N'editer_token', N'9f4141b241184aa8b88ca0dfc3ebc7c5', N'aa9244ec196b037b74e22e52af44d3ed', N'8a783040f0a843f63b708db14832eeeb', N'2016-12-27 10:14:55.453')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'aa9244ec196b037b74e22e52af44d3ed8ba25f0eb85fd03fe6c8f4895b577889', N'editer_token', N'c5b159d087dd400692d1443afb9a0547', N'aa9244ec196b037b74e22e52af44d3ed', N'8ba25f0eb85fd03fe6c8f4895b577889', N'2016-12-15 21:19:55.283')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'aa9244ec196b037b74e22e52af44d3ed8bf4a3be69712bab3938f8222a12d0b9', N'editer_token', N'd00d9d34879e41438dd6f84cde2e3758', N'aa9244ec196b037b74e22e52af44d3ed', N'8bf4a3be69712bab3938f8222a12d0b9', N'2017-01-09 14:33:17.543')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'aa9244ec196b037b74e22e52af44d3ed90f7e885f9c90297981a581eede14152', N'editer_token', N'af333b8964044edba4e394651f9153ab', N'aa9244ec196b037b74e22e52af44d3ed', N'90f7e885f9c90297981a581eede14152', N'2017-01-19 15:10:21.660')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'aa9244ec196b037b74e22e52af44d3ed91c1b059431a4e8c9a44aa38560ff833', N'editer_token', N'bad96d2c41e24754a410a5ccdcfb7026', N'aa9244ec196b037b74e22e52af44d3ed', N'91c1b059431a4e8c9a44aa38560ff833', N'2017-02-15 15:25:27.110')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'aa9244ec196b037b74e22e52af44d3ed91f901ffb736c836c8c8110f202714e7', N'editer_token', N'ba258f28f4d14c368b1802c41e7087c8', N'aa9244ec196b037b74e22e52af44d3ed', N'91f901ffb736c836c8c8110f202714e7', N'2017-02-08 18:29:37.370')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'aa9244ec196b037b74e22e52af44d3ed92b5f0b8a3d469c8409731ac69117b64', N'editer_token', N'1f6391e41986416dbaf8ddbe7a569b34', N'aa9244ec196b037b74e22e52af44d3ed', N'92b5f0b8a3d469c8409731ac69117b64', N'2016-12-16 13:35:28.970')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'aa9244ec196b037b74e22e52af44d3ed94dde56165f13ac9067e1da0359b94d9', N'editer_token', N'b7b195d1b47a4c93b570aa5f54a42454', N'aa9244ec196b037b74e22e52af44d3ed', N'94dde56165f13ac9067e1da0359b94d9', N'2016-11-28 14:43:58.280')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'aa9244ec196b037b74e22e52af44d3ed970a965cd43aa85e6034f70eca6f5ced', N'editer_token', N'734256f5f6b84455bc9ca553724e5aec', N'aa9244ec196b037b74e22e52af44d3ed', N'970a965cd43aa85e6034f70eca6f5ced', N'2016-11-25 15:29:42.600')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'aa9244ec196b037b74e22e52af44d3ed98fdba0f4bb608436e2d1bd82534a3ac', N'editer_token', N'6117d1a7fd5947a6b5ef2e2d9dab30c7', N'aa9244ec196b037b74e22e52af44d3ed', N'98fdba0f4bb608436e2d1bd82534a3ac', N'2016-12-09 14:21:18.580')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'aa9244ec196b037b74e22e52af44d3ed9bd2c03c41a9a59f0753dfdfb527e26d', N'editer_token', N'e2f260009e2b4da89ab7a4c8b698a202', N'aa9244ec196b037b74e22e52af44d3ed', N'9bd2c03c41a9a59f0753dfdfb527e26d', N'2016-12-13 18:56:58.193')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'aa9244ec196b037b74e22e52af44d3ed9d3d5caa5b126fef6d2d124db1c3c787', N'editer_token', N'7516fb0596f2472a8647d497b5b540a7', N'aa9244ec196b037b74e22e52af44d3ed', N'9d3d5caa5b126fef6d2d124db1c3c787', N'2017-01-19 21:24:03.383')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'aa9244ec196b037b74e22e52af44d3ed9f77a6462ae54d41434ad3bfc2384423', N'editer_token', N'c6c1c7d80baa42b08991601d44264518', N'aa9244ec196b037b74e22e52af44d3ed', N'9f77a6462ae54d41434ad3bfc2384423', N'2017-03-21 16:54:56.013')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'aa9244ec196b037b74e22e52af44d3eda05be2b3af72a8d9a4e3ddd2098808e1', N'editer_token', N'db0edf309f4d4124a078c56b2edef74b', N'aa9244ec196b037b74e22e52af44d3ed', N'a05be2b3af72a8d9a4e3ddd2098808e1', N'2017-02-09 21:10:01.610')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'aa9244ec196b037b74e22e52af44d3eda3f130c6c99d349fdd18d030bc913d2a', N'editer_token', N'53f9196360d545efb2d30e2b08f83256', N'aa9244ec196b037b74e22e52af44d3ed', N'a3f130c6c99d349fdd18d030bc913d2a', N'2016-12-27 10:16:15.183')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'aa9244ec196b037b74e22e52af44d3eda458e5276a1562f810851198defe381a', N'editer_token', N'eccff67942ef425186d87da075e00d82', N'aa9244ec196b037b74e22e52af44d3ed', N'a458e5276a1562f810851198defe381a', N'2017-01-22 11:11:57.703')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'aa9244ec196b037b74e22e52af44d3eda5ceef4bfbd914ec320b17b942039d7c', N'editer_token', N'f953e40dd1bf44e1af91a6f543449815', N'aa9244ec196b037b74e22e52af44d3ed', N'a5ceef4bfbd914ec320b17b942039d7c', N'2017-03-14 15:09:45.500')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'aa9244ec196b037b74e22e52af44d3edafd4c7aec49a55888979b4226379ea77', N'editer_token', N'd3c4ae6dfeab498e90953fa7c8112bf2', N'aa9244ec196b037b74e22e52af44d3ed', N'afd4c7aec49a55888979b4226379ea77', N'2016-12-22 14:08:37.357')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'aa9244ec196b037b74e22e52af44d3edb2bd8ab6eb2bc696ffed67363adfb884', N'editer_token', N'4bcd418e2d244d15be1428e3b15ab631', N'aa9244ec196b037b74e22e52af44d3ed', N'b2bd8ab6eb2bc696ffed67363adfb884', N'2016-11-28 21:14:14.897')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'aa9244ec196b037b74e22e52af44d3edb4cbabf7eb088ed73ec0e065afa38dbb', N'editer_token', N'da17170c04384f02be5651b6d310ae56', N'aa9244ec196b037b74e22e52af44d3ed', N'b4cbabf7eb088ed73ec0e065afa38dbb', N'2016-12-20 18:08:55.043')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'aa9244ec196b037b74e22e52af44d3edb5e7d0e2802b37ab2586a27d0804620f', N'editer_token', N'5f67bd223aad4ca4849b9f9242ca4691', N'aa9244ec196b037b74e22e52af44d3ed', N'b5e7d0e2802b37ab2586a27d0804620f', N'2017-03-22 15:51:32.540')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'aa9244ec196b037b74e22e52af44d3edb61ef3bcf4e76cb7988e5e19d3e225aa', N'editer_token', N'64421ca400e9470abcb763ad15080440', N'aa9244ec196b037b74e22e52af44d3ed', N'b61ef3bcf4e76cb7988e5e19d3e225aa', N'2016-11-28 09:14:30.100')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'aa9244ec196b037b74e22e52af44d3edb674a75bf6e60993e5deb46b53ba30a9', N'editer_token', N'a18e0d1b8287419d8cdf8c191337f745', N'aa9244ec196b037b74e22e52af44d3ed', N'b674a75bf6e60993e5deb46b53ba30a9', N'2017-01-19 13:58:00.007')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'aa9244ec196b037b74e22e52af44d3edb7382181f5c86383f43459a7167fb988', N'editer_token', N'deb94df9d6db47f6a37cbab87fc4ad5b', N'aa9244ec196b037b74e22e52af44d3ed', N'b7382181f5c86383f43459a7167fb988', N'2017-02-23 17:18:37.337')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'aa9244ec196b037b74e22e52af44d3edb802673f3a85e241b29d934aa615db1b', N'editer_token', N'ea21e4eb12b44710b57b42467f52a9b4', N'aa9244ec196b037b74e22e52af44d3ed', N'b802673f3a85e241b29d934aa615db1b', N'2017-02-21 14:17:14.350')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'aa9244ec196b037b74e22e52af44d3edb8b39b0226def150370ac965ede19a47', N'editer_token', N'f5bec9c777cb412ca8ec954221eb3507', N'aa9244ec196b037b74e22e52af44d3ed', N'b8b39b0226def150370ac965ede19a47', N'2017-03-14 15:43:35.947')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'aa9244ec196b037b74e22e52af44d3edb9292fc259c4679a7ec53cbf76c1e480', N'editer_token', N'3df77da5eb38485192da9fae4a75aa0c', N'aa9244ec196b037b74e22e52af44d3ed', N'b9292fc259c4679a7ec53cbf76c1e480', N'2017-03-04 19:03:12.980')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'aa9244ec196b037b74e22e52af44d3edb9a72ec9ce5144ded5eddf48c0e1572c', N'editer_token', N'bb9b75fcc0f34a799f4827605065b489', N'aa9244ec196b037b74e22e52af44d3ed', N'b9a72ec9ce5144ded5eddf48c0e1572c', N'2017-01-18 10:26:58.077')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'aa9244ec196b037b74e22e52af44d3edb9d2602fb3e8fc943ce21ee04a918757', N'editer_token', N'fd098311a2bb42fe97621bbfe1b4bcfe', N'aa9244ec196b037b74e22e52af44d3ed', N'b9d2602fb3e8fc943ce21ee04a918757', N'2017-02-06 17:21:13.687')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'aa9244ec196b037b74e22e52af44d3edbb8ec1d54e1008a8daf3f61a20c6d29e', N'editer_token', N'8f273621258244b4b8d89f06e72e05ca', N'aa9244ec196b037b74e22e52af44d3ed', N'bb8ec1d54e1008a8daf3f61a20c6d29e', N'2017-02-15 16:37:52.800')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'aa9244ec196b037b74e22e52af44d3edbbb0491091c7eb1d443da51af2fb1d50', N'editer_token', N'ac6a2507b7bb4d069555a6c6cd27f1c5', N'aa9244ec196b037b74e22e52af44d3ed', N'bbb0491091c7eb1d443da51af2fb1d50', N'2017-03-28 15:55:49.280')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'aa9244ec196b037b74e22e52af44d3edbc2e0d2806839139ad384d19c7b6f025', N'editer_token', N'9a811b3b74d44977874a5c07b005ab4c', N'aa9244ec196b037b74e22e52af44d3ed', N'bc2e0d2806839139ad384d19c7b6f025', N'2017-02-21 18:06:30.230')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'aa9244ec196b037b74e22e52af44d3edbedede25d5bd8e016656e9152a8e2415', N'editer_token', N'pvZjMsP3T02MDW4sxK7n/g==', N'aa9244ec196b037b74e22e52af44d3ed', N'bedede25d5bd8e016656e9152a8e2415', N'2016-12-26 20:46:23.333')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'aa9244ec196b037b74e22e52af44d3edbfd3ff10ca2d5a2d1a74cf17e053de0c', N'editer_token', N'16ca7b525735498cbbdcbe5985f227b8', N'aa9244ec196b037b74e22e52af44d3ed', N'bfd3ff10ca2d5a2d1a74cf17e053de0c', N'2017-01-16 15:11:07.547')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'aa9244ec196b037b74e22e52af44d3edc2f78ac8d83ee7c5e012081afd27cf25', N'editer_token', N'4636a45751794a329448ccd044f2a745', N'aa9244ec196b037b74e22e52af44d3ed', N'c2f78ac8d83ee7c5e012081afd27cf25', N'2016-12-22 09:41:34.743')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'aa9244ec196b037b74e22e52af44d3edc35a20f737644063069df9cb5fc51537', N'editer_token', N'63f93b0fd0cb4b2bb051b398088219ac', N'aa9244ec196b037b74e22e52af44d3ed', N'c35a20f737644063069df9cb5fc51537', N'2017-03-28 16:58:15.887')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'aa9244ec196b037b74e22e52af44d3edc44f62ea1b2194a1e752a59c61017c54', N'editer_token', N'b443283502324ff6b838b69c055c5c57', N'aa9244ec196b037b74e22e52af44d3ed', N'c44f62ea1b2194a1e752a59c61017c54', N'2016-12-27 20:08:42.403')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'aa9244ec196b037b74e22e52af44d3edc47aad1ccf1e846e918f95a4dcd7a9ce', N'editer_token', N'd03f7ef91fbe407fbce1c57cc73b49fa', N'aa9244ec196b037b74e22e52af44d3ed', N'c47aad1ccf1e846e918f95a4dcd7a9ce', N'2016-11-28 12:46:51.843')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'aa9244ec196b037b74e22e52af44d3edc543dd054e9c8a354771a3f21b9c18c6', N'editer_token', N'4854e1a7283e4694ae438db04f1a0f5b', N'aa9244ec196b037b74e22e52af44d3ed', N'c543dd054e9c8a354771a3f21b9c18c6', N'2017-02-08 15:30:34.853')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'aa9244ec196b037b74e22e52af44d3edc63dc41e8c7f1012301e78a54ded71e9', N'editer_token', N'b9735c2815f44679a705dbd92a674291', N'aa9244ec196b037b74e22e52af44d3ed', N'c63dc41e8c7f1012301e78a54ded71e9', N'2016-12-16 19:34:00.390')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'aa9244ec196b037b74e22e52af44d3edc7a8080ecf8c51a6de099f34b6f0dde0', N'editer_token', N'909d22159d494bc891c548dc583d3adb', N'aa9244ec196b037b74e22e52af44d3ed', N'c7a8080ecf8c51a6de099f34b6f0dde0', N'2017-03-16 11:45:41.680')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'aa9244ec196b037b74e22e52af44d3edc8a595452c8b75299c540cd1e744a5d6', N'editer_token', N'e277fc41731a4f1ba0bc60b3ece3c15d', N'aa9244ec196b037b74e22e52af44d3ed', N'c8a595452c8b75299c540cd1e744a5d6', N'2016-12-13 18:56:24.990')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'aa9244ec196b037b74e22e52af44d3edca04c626dee8bb5c39509eecaf42d6af', N'editer_token', N'2a3ad38226ae47aab3115417aede7ee4', N'aa9244ec196b037b74e22e52af44d3ed', N'ca04c626dee8bb5c39509eecaf42d6af', N'2016-12-20 14:58:29.873')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'aa9244ec196b037b74e22e52af44d3edcadc208ee8a6bc1344a51f0875b0a91f', N'editer_token', N'86595c49b9d04186a61bc89408946e6d', N'aa9244ec196b037b74e22e52af44d3ed', N'cadc208ee8a6bc1344a51f0875b0a91f', N'2017-02-15 18:27:32.500')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'aa9244ec196b037b74e22e52af44d3edcbcb2d91a0923f9f524dd4827f16cab3', N'editer_token', N'bab4c8ed8fb5452f86a644333c298c6f', N'aa9244ec196b037b74e22e52af44d3ed', N'cbcb2d91a0923f9f524dd4827f16cab3', N'2016-12-13 11:25:25.300')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'aa9244ec196b037b74e22e52af44d3edcbe938c100b95a882e65ce20e1a4a27b', N'editer_token', N'a237a00d52634e0d826a94df155e1d58', N'aa9244ec196b037b74e22e52af44d3ed', N'cbe938c100b95a882e65ce20e1a4a27b', N'2017-02-15 17:02:35.323')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'aa9244ec196b037b74e22e52af44d3edcbf63ecc3a60e6102a360d89e1dae9aa', N'editer_token', N'df3bafdf38274ca088fcd5caba577d27', N'aa9244ec196b037b74e22e52af44d3ed', N'cbf63ecc3a60e6102a360d89e1dae9aa', N'2016-12-22 15:48:13.167')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'aa9244ec196b037b74e22e52af44d3edcd166ba1a0f45686a5c7211e6ff76116', N'editer_token', N'6d55596a95c4434fb6b9f4b65c0f4e4f', N'aa9244ec196b037b74e22e52af44d3ed', N'cd166ba1a0f45686a5c7211e6ff76116', N'2016-12-16 19:35:04.100')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'aa9244ec196b037b74e22e52af44d3edcf478e305b3d52f424434da79eae5e7b', N'editer_token', N'dd55b7a2233245bc8dd7759615764550', N'aa9244ec196b037b74e22e52af44d3ed', N'cf478e305b3d52f424434da79eae5e7b', N'2016-11-29 13:06:50.450')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'aa9244ec196b037b74e22e52af44d3edcf59e228685d6a22e2771fef2b9df000', N'editer_token', N'321b9efdf8d7420da175e1456ffaad43', N'aa9244ec196b037b74e22e52af44d3ed', N'cf59e228685d6a22e2771fef2b9df000', N'2017-01-16 15:30:49.130')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'aa9244ec196b037b74e22e52af44d3edd165d4dde9cf28f6541b52afbc72bebb', N'editer_token', N'a657e3cac1cb4002993a584ddb6e4723', N'aa9244ec196b037b74e22e52af44d3ed', N'd165d4dde9cf28f6541b52afbc72bebb', N'2016-12-27 14:18:34.673')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'aa9244ec196b037b74e22e52af44d3edd5e32be65f1d4d63de1f557c198e09a5', N'editer_token', N'a6f117d678094d4d8d9c9220dc2d662e', N'aa9244ec196b037b74e22e52af44d3ed', N'd5e32be65f1d4d63de1f557c198e09a5', N'2016-11-28 09:58:15.270')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'aa9244ec196b037b74e22e52af44d3edd9b56a2d7cca8160f5f6e76275148196', N'editer_token', N'2aedf2f674284e0989f449588fc7ce22', N'aa9244ec196b037b74e22e52af44d3ed', N'd9b56a2d7cca8160f5f6e76275148196', N'2017-02-04 18:16:27.290')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'aa9244ec196b037b74e22e52af44d3eddf0ea3ea9e6f33f554b97321158068c9', N'editer_token', N'05c59bd792eb4d43bb2815097fa760b6', N'aa9244ec196b037b74e22e52af44d3ed', N'df0ea3ea9e6f33f554b97321158068c9', N'2016-12-01 09:56:20.960')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'aa9244ec196b037b74e22e52af44d3ede1d65855fb546db4735077e646bef80f', N'editer_token', N'b247f2ac143444e6938d970016e973fd', N'aa9244ec196b037b74e22e52af44d3ed', N'e1d65855fb546db4735077e646bef80f', N'2017-03-27 17:25:23.930')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'aa9244ec196b037b74e22e52af44d3eded8aa071278058315b2a8e33e68ec049', N'editer_token', N'7c2cfddf99964477bde3118b2d61bb30', N'aa9244ec196b037b74e22e52af44d3ed', N'ed8aa071278058315b2a8e33e68ec049', N'2017-02-09 20:43:59.210')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'aa9244ec196b037b74e22e52af44d3edef281be0c6b70064d8177f71432b68ce', N'editer_token', N'9510c51e1a62414c9d905cdbd07110e5', N'aa9244ec196b037b74e22e52af44d3ed', N'ef281be0c6b70064d8177f71432b68ce', N'2017-02-09 16:23:51.533')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'aa9244ec196b037b74e22e52af44d3edef5db07b81ec2f474ed8b34bd932218f', N'editer_token', N'c777960f9165452ab8914814540111d6', N'aa9244ec196b037b74e22e52af44d3ed', N'ef5db07b81ec2f474ed8b34bd932218f', N'2016-11-28 10:17:33.267')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'aa9244ec196b037b74e22e52af44d3edf125a72f636aa9fdff74048c2f1964c4', N'editer_token', N'77291c6bdf8b4c00a6349c0b2a6e2b01', N'aa9244ec196b037b74e22e52af44d3ed', N'f125a72f636aa9fdff74048c2f1964c4', N'2016-12-27 10:13:43.713')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'aa9244ec196b037b74e22e52af44d3edf484424c3d1192714af9b3ab37c059fe', N'editer_token', N'6609aa11cfc049b8a5a854499f53d44e', N'aa9244ec196b037b74e22e52af44d3ed', N'f484424c3d1192714af9b3ab37c059fe', N'2016-12-27 10:15:04.497')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'aa9244ec196b037b74e22e52af44d3edf4d8136bbbcd18730a285d46b7298fdd', N'editer_token', N'a599517ad9f843e495c87819b4f6a63b', N'aa9244ec196b037b74e22e52af44d3ed', N'f4d8136bbbcd18730a285d46b7298fdd', N'2017-03-21 11:19:13.923')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'aa9244ec196b037b74e22e52af44d3edfc9b5823765d83533f58782ffb345b00', N'editer_token', N'1141bc2f422840498f1b9a0c21346f46', N'aa9244ec196b037b74e22e52af44d3ed', N'fc9b5823765d83533f58782ffb345b00', N'2017-02-15 15:30:41.430')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'aa9244ec196b037b74e22e52af44d3edfd54e5130ad400c1273680468d1d2d6f', N'editer_token', N'8996b4ad6adf422389ffe80b0e6085f3', N'aa9244ec196b037b74e22e52af44d3ed', N'fd54e5130ad400c1273680468d1d2d6f', N'2016-12-21 18:15:54.190')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'ab24f9913a2d8dec8d9aaa2baed8c36a58d62d8fdeedb93ec68a69321de9159e', N'oqTz6M16sEuUsNJTQNRIsA==', N'0a0929ccc0e6470399f4b7ee41811036', N'ab24f9913a2d8dec8d9aaa2baed8c36a', N'58d62d8fdeedb93ec68a69321de9159e', N'2017-02-20 17:36:37.400')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'ab5fbcee5d9cbae891525acc10a1d24d838f46faf9a5cc1ba86d68b5ab61eb70', N'migl8NPAEUyLYY3IM5zt6w==', N'9897f10fd1d7434f84b21f714f0d3cef', N'ab5fbcee5d9cbae891525acc10a1d24d', N'838f46faf9a5cc1ba86d68b5ab61eb70', N'2016-12-27 22:42:10.763')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'abd3159275a3904b1ee3f2acc30435edbe59152dd02e39ef3cc78b7aea530d5f', N'mZ2LEkwXt0a8XR1bg30wfQ==', N'e03767494002472cb66cc3d5eb8ecb51', N'abd3159275a3904b1ee3f2acc30435ed', N'be59152dd02e39ef3cc78b7aea530d5f', N'2017-02-28 10:24:47.113')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'ac1fd4c05c6e3e86d3269346d39858250dd402efb2d3b3d8992adddf2b7bf5eb', N'KsFEUP/dbEi48yM0CTnSTw==', N'33a61681b34a482b968f85ea967717df', N'ac1fd4c05c6e3e86d3269346d3985825', N'0dd402efb2d3b3d8992adddf2b7bf5eb', N'2016-12-28 11:05:06.460')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'add394540ce4b36093af3d70c2e8093c6aa0309392651112d29b3478333082d6', N'sHqkbSUsZk2fFcbu0I28HQ==', N'5dd5a590e97e40aba36d8d2f646d0798', N'add394540ce4b36093af3d70c2e8093c', N'6aa0309392651112d29b3478333082d6', N'2017-02-08 15:00:20.890')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'ae705c5bb688eb968ca08d7144a90e104282baed3f94d5bd02fecaedf9c14f51', N'sdmGHb0tREiQNPh55AxHmw==', N'e60fd4254ee34997b7753e48b9431e98', N'ae705c5bb688eb968ca08d7144a90e10', N'4282baed3f94d5bd02fecaedf9c14f51', N'2017-02-09 11:40:59.670')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'ae9404749278de556ef8e3c10ac3859401c8b5f3c121421f04476d567e473a10', N'2ag5jBZ10Eib87xpn0K2+Q==', N'b96c72186e45486280bb99257cd02aea', N'ae9404749278de556ef8e3c10ac38594', N'01c8b5f3c121421f04476d567e473a10', N'2017-02-23 17:42:33.460')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'aed1c69669aa5c01089361e1ac8a7e4c8bcb76f46d9e6119b1bbdc7ad855a0c7', N'wHtRNE4A6kGOZ8REzcMHrQ==', N'cb0fd7fedb5f4a2abe19670c49e39a3e', N'aed1c69669aa5c01089361e1ac8a7e4c', N'8bcb76f46d9e6119b1bbdc7ad855a0c7', N'2016-12-27 12:26:44.227')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'af2bff84df5d286e9f87abf2f2cc326c5d7cc9c350e8d4ca9b9c70f5fe4dab52', N'rzJuskTK/EaojRD46sok3g==', N'9535dbbb193e48d5a01592704be48c41', N'af2bff84df5d286e9f87abf2f2cc326c', N'5d7cc9c350e8d4ca9b9c70f5fe4dab52', N'2017-02-13 10:58:40.770')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'af77b53f0cc43a81cf85a762c433189c9a88c1134edef6d000ea568f59b26369', N'e+z/AW7KIEKeuB6ixW4llA==', N'a6a73a6ad5564bce8e0b2d274c7084d0', N'af77b53f0cc43a81cf85a762c433189c', N'9a88c1134edef6d000ea568f59b26369', N'2017-03-02 11:19:31.940')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'afb8d48490f0df8feb37ba337ae5cf25a500b68462b49bf6bd07b843f33d26df', N'2AIE56nklUucWS3tvzMjXw==', N'1548c623013c445fb274cc82f7ee147c', N'afb8d48490f0df8feb37ba337ae5cf25', N'a500b68462b49bf6bd07b843f33d26df', N'2017-02-18 19:43:16.073')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'afc7f2d2a222fb3a4b88e05e74259db8eda59a37a24d9563a37ec012aa5cf0bb', N'owvZHUzr8U+CrcO3S7Xspg==', N'13b2e0db8f1d48a2b488b6853ef628a3', N'afc7f2d2a222fb3a4b88e05e74259db8', N'eda59a37a24d9563a37ec012aa5cf0bb', N'2017-01-05 16:44:38.410')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'b04be18be2c59996aadc9aacb2b706b784eb6404e62641a076a6b1f93a7b2db4', N'mNUd/ONzdUaGZCSOluKDAg==', N'749e341168e34287bfb85dde1032d100', N'b04be18be2c59996aadc9aacb2b706b7', N'84eb6404e62641a076a6b1f93a7b2db4', N'2017-02-27 11:42:02.370')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'b0d4b4cd1e16707cded9daa2fc852d0c9b9031932a5d4d834a681c2b1537c622', N'7swhBSPqAkGiw51vbDFcGw==', N'8a9e5a071d2341a7b06b638fa989fd19', N'b0d4b4cd1e16707cded9daa2fc852d0c', N'9b9031932a5d4d834a681c2b1537c622', N'2017-01-17 17:18:52.777')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'b10fd9750a9b08df691be489b36e2d133c2cd7662b2b96bf09e8590ee9640938', N'OD8ozQ79FUycy1qDIC68+w==', N'9caac09f155d420ab35c4870de580539', N'b10fd9750a9b08df691be489b36e2d13', N'3c2cd7662b2b96bf09e8590ee9640938', N'2017-03-07 14:49:53.260')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'b12f77beed0709514f571d2d732e5e421953cc04efc05ddd68d9a13fc1f930d4', N'ynzSXqg7Uk6lsj/p63sg2w==', N'd6699205e20243328beb9b282c62289b', N'b12f77beed0709514f571d2d732e5e42', N'1953cc04efc05ddd68d9a13fc1f930d4', N'2017-03-23 19:23:17.997')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'b14b55812bcce07f12524729dbffd67760e91068ceafcb1bf4876092af7c3119', N'PdFHrssep0yfLlyV1BAzdQ==', N'29e2549136194323a37c71f8ab9fc17f', N'b14b55812bcce07f12524729dbffd677', N'60e91068ceafcb1bf4876092af7c3119', N'2017-03-07 10:20:24.870')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'b14d0719b5de8a2b97f3117845565ea94ec9770902aeb39c66f736f26d68151e', N'R1qQKUYjukWwY1guGHfS3w==', N'4791ad0606b646afb4e20af873f42aa7', N'b14d0719b5de8a2b97f3117845565ea9', N'4ec9770902aeb39c66f736f26d68151e', N'2017-03-17 10:52:38.973')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'b1fa1db489af7ccd711c3f4f918bd0d49a7d14ec1b2db3de07b6dec388259725', N'Z5TlLsVx7UaadxJGc9Nh9w==', N'b25d11211643414dbe64844d795b3662', N'b1fa1db489af7ccd711c3f4f918bd0d4', N'9a7d14ec1b2db3de07b6dec388259725', N'2017-03-23 11:34:19.703')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'b2e64d53c204a88064934f08ce1cedaeb685b121c817b24d37347e7d1b6bc733', N'+OiceSKp5Ua+95E7/NwypQ==', N'd69847306d3b431297c591b11b45656b', N'b2e64d53c204a88064934f08ce1cedae', N'b685b121c817b24d37347e7d1b6bc733', N'2016-12-28 01:06:21.867')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'b2ea6b89613609cea23c1a422833044934bafebf3642117a8f81710acc4f6f9d', N'dzihWa0wekm5uyMJxoh/0A==', N'ea7e2b74cb1c47b2bdb0da64ca7b32ee', N'b2ea6b89613609cea23c1a4228330449', N'34bafebf3642117a8f81710acc4f6f9d', N'2017-03-17 10:16:43.653')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'b32b618a2987cc8ad54e0dea4b0dfe38fff8241a3bce6eb1d6d6802b9feada52', N'qj4XyJi4HkO5mmRf6a/WZg==', N'd755f9e7f29048458bfbd79abce4ba6c', N'b32b618a2987cc8ad54e0dea4b0dfe38', N'fff8241a3bce6eb1d6d6802b9feada52', N'2017-03-01 14:52:10.807')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'b40213870e437be3a3d9f43afc39cad64d8a37e0090b077a0a1829540430cb9e', N'psFf4DDJQ0Cy4V0oeAnC8A==', N'26590edcfc5e47e590adc52e62d0eca3', N'b40213870e437be3a3d9f43afc39cad6', N'4d8a37e0090b077a0a1829540430cb9e', N'2017-02-21 18:02:18.620')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'b479d4b8ed86630b84e0a423722fbc5ba1dcce021802d071014c49ce0b176bc1', N'R3a41NxrtEqoFqYOnHviwg==', N'5924297c5f9244aab61f6f1a58658207', N'b479d4b8ed86630b84e0a423722fbc5b', N'a1dcce021802d071014c49ce0b176bc1', N'2017-02-09 16:05:16.900')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'b494d8e7d5fe9337e88ebd1e4871250d74d47c3c57606e91dcc478e0fcaeb14c', N'Ouz8yvAcRUi/FKj1+cFFAw==', N'dcc9e7a2f22648f8b0b02f40deed24d1', N'b494d8e7d5fe9337e88ebd1e4871250d', N'74d47c3c57606e91dcc478e0fcaeb14c', N'2017-01-11 11:00:32.277')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'b7784c8cb8e6f2bbfed5be3c5f9bf74fe2ef4d7b7f6f56ffa3bc941352f65cbf', N'MMUQtwoI0UOR4zxpKyzy8A==', N'1026940df3bf425795a68bcc1fa64ee7', N'b7784c8cb8e6f2bbfed5be3c5f9bf74f', N'e2ef4d7b7f6f56ffa3bc941352f65cbf', N'2017-02-26 14:51:44.617')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'b779e21816a15172dc020c86e275381bb89d853187a6ee93b113edd02ba779f2', N'b++y99KtXk+MUjFfYTyzlA==', N'858e40475c5f4000afd10b27ace570a1', N'b779e21816a15172dc020c86e275381b', N'b89d853187a6ee93b113edd02ba779f2', N'2017-03-03 16:34:23.737')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'b84e8d4bceacbe9264ef01130feaa0ca955fbbfd0a14bc78b80c97371ff9406d', N'm43xEKniAkS58NYKWg2bOg==', N'7a55a3d762c74d27b6e7a2714c5f0290', N'b84e8d4bceacbe9264ef01130feaa0ca', N'955fbbfd0a14bc78b80c97371ff9406d', N'2017-03-03 17:18:00.310')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'b89c6d01518be75610cd0d86ed6df1950bdc4c409c60e92b74fb11cf07fca12e', N'9wUZbwcE8Emvhn8NXF4XYg==', N'c0ba2aaf166c4de88f9b3b11bcec91a0', N'b89c6d01518be75610cd0d86ed6df195', N'0bdc4c409c60e92b74fb11cf07fca12e', N'2017-02-10 14:53:09.987')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'b9d710de085dfc82ea9a3f1c2c576a8a9cf2654fc912c00e5e5b097680c5582d', N'LDNR0PHsS0me40rp4as/0A==', N'3dd534aa826c4523b75f99fda6417c03', N'b9d710de085dfc82ea9a3f1c2c576a8a', N'9cf2654fc912c00e5e5b097680c5582d', N'2017-02-11 14:08:27.340')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'ba2c8136d262de762f351466faccc60671fb719a5d88c3660ab7ce7f2c2f0466', N'meKd0jtGeUy6jNWY3Pp6Fw==', N'39720febe0c24312b95bb6f2e26133b7', N'ba2c8136d262de762f351466faccc606', N'71fb719a5d88c3660ab7ce7f2c2f0466', N'2017-02-22 18:46:28.010')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'ba68c8f1d9098e0e6c1eaf64f2a7bf6f18d001c16d97a6f6a337437309a102c3', N'oiXpUxDFi0GbUepBRCa8zw==', N'aa396a29b4ea4ec78b882fb6dfa7b849', N'ba68c8f1d9098e0e6c1eaf64f2a7bf6f', N'18d001c16d97a6f6a337437309a102c3', N'2016-12-30 16:54:40.087')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'bb0eadf9adf9c6af99febdab7f897f766b73b6456a78afcaa0249c49b817569e', N'siJmiskfhkCxEfe+U3M2sw==', N'3d880665c0bc4f4ea2415b727b7737d0', N'bb0eadf9adf9c6af99febdab7f897f76', N'6b73b6456a78afcaa0249c49b817569e', N'2017-01-17 15:59:40.070')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'bb67c9b9ce2d1eead5ea2748e19c531318fbc4fb04b0964c4762a6a4e880994f', N'yonrJMlT1U+zWAo8SoKNGw==', N'9CCAE69A-08F0-47E7-8EC7-36BCE79F5CB8', N'bb67c9b9ce2d1eead5ea2748e19c5313', N'18fbc4fb04b0964c4762a6a4e880994f', N'2017-03-24 16:56:20.493')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'bb6c210b534a747a293c22f6a0fe809a903f1de49b2bfae9b56c386d5bbbad53', N'414JoM9I20KlXnICPHkQVQ==', N'eff90bea320b45b88a79e538e423e48a', N'bb6c210b534a747a293c22f6a0fe809a', N'903f1de49b2bfae9b56c386d5bbbad53', N'2017-02-10 23:00:25.863')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'bc2af881c5a4229eb88a9cf2ea8d2643b003d9f6be29245eb3fc82e1e0d42ac8', N'5yLBw6OwSECs4euigdpRDw==', N'b10595679d744d5ba78c6fe94839ff4d', N'bc2af881c5a4229eb88a9cf2ea8d2643', N'b003d9f6be29245eb3fc82e1e0d42ac8', N'2017-03-24 10:29:15.740')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'bc5742b04389b8b8cd5e49906ce5c6fd11ef7af18c8d0c9e2c0feb12951da576', N'i7AXjGHPNkWPDet4wDTpxA==', N'184811907ff34a2f9b6eac6c45b5bc44', N'bc5742b04389b8b8cd5e49906ce5c6fd', N'11ef7af18c8d0c9e2c0feb12951da576', N'2016-12-27 10:09:59.320')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'bc7fb46c0050e78f1533ce0a42d0807002186e2d3d102cfa4aa4fbe6f429e677', N'ZuMd8oVgXEK6GVlJZk6dKw==', N'7aa6f81d1f014ecfa991fd4cc0034b58', N'bc7fb46c0050e78f1533ce0a42d08070', N'02186e2d3d102cfa4aa4fbe6f429e677', N'2016-12-27 10:11:44.583')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'bcc4316ee59adce49b3204754716f5530a67c4ee81fa7ff52309e83c112900a3', N'PBUK9DqOFkK6YKaQuLTAUg==', N'ee35704726f147f7bd1a230b578262eb', N'bcc4316ee59adce49b3204754716f553', N'0a67c4ee81fa7ff52309e83c112900a3', N'2017-03-01 11:49:39.123')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'bd2490f97220aba51d5a78e5c4c1774fccc93e7e34c80ca679185816e68d3802', N'oL0XTqG2gUqBZBZa4zRWnA==', N'0b2267c97a0b4d9a9e20469c6e4f36a5', N'bd2490f97220aba51d5a78e5c4c1774f', N'ccc93e7e34c80ca679185816e68d3802', N'2016-12-27 16:13:32.017')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'bf0a0ce64bc6d51bc8272c3a4e57c66b58d5118e70ea96855bec74cb3bc86c3a', N'wuQ0jhQhf0Or+dxWvLJOfQ==', N'd036e367aeae4ef4a1a8eac757845727', N'bf0a0ce64bc6d51bc8272c3a4e57c66b', N'58d5118e70ea96855bec74cb3bc86c3a', N'2016-12-28 15:58:36.587')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'bfe949b209ca9eb4e9b313370381b22b742505455d28e4382f2eec921b9f9c8d', N'xVun+PnS1U6zBDkOigUjQg==', N'c659af9de5dc46369724d3c439ad1ee3', N'bfe949b209ca9eb4e9b313370381b22b', N'742505455d28e4382f2eec921b9f9c8d', N'2017-01-17 13:51:13.310')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'bfea4fa70775ade330f61a98949b83e7e93ee0b3077d5ad4058c87665d5dd7a0', N'7KPVY1QlRkCZiOuiZFNw8g==', N'9ef50bf081594c328dc6f1c315a2a0ff', N'bfea4fa70775ade330f61a98949b83e7', N'e93ee0b3077d5ad4058c87665d5dd7a0', N'2017-03-22 22:10:01.947')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'c05f9eae3a89482737318f981cb8df19070fe35b9ce32da52913194c5521c4d8', N'y09jpb2m+0a9m0ue0UvJTQ==', N'af8885e3f6ba42ddacf3ce401aaef47e', N'c05f9eae3a89482737318f981cb8df19', N'070fe35b9ce32da52913194c5521c4d8', N'2017-02-10 16:29:52.900')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'c15102d8b666f1a9629bb3a6b0ff0b2f71ed415e6517c34e46a24aacdbcbb561', N'sGrFeiUFV0aVnk/5rO/cMg==', N'c404f4cdde7e4881936248628e5d036b', N'c15102d8b666f1a9629bb3a6b0ff0b2f', N'71ed415e6517c34e46a24aacdbcbb561', N'2017-03-21 14:38:16.857')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'c1970d1d9955660e7fbac8cd4743e4f105f4605564a46f0f4463af8db8804da1', N'hy2gRP0vs0+mzOg5eFJ1Ng==', N'261bf64260f1453ab89dff420ed9b23e', N'c1970d1d9955660e7fbac8cd4743e4f1', N'05f4605564a46f0f4463af8db8804da1', N'2017-02-09 15:22:27.570')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'c1ac1bccfaa78db8ed6e8a49274e942976d92652190b617d40a865ec9cfa1140', N'onnkzkUj2Uu9OCghiZj2Rg==', N'9302902155f14c36b7c4f0644de23369', N'c1ac1bccfaa78db8ed6e8a49274e9429', N'76d92652190b617d40a865ec9cfa1140', N'2017-02-08 11:40:50.480')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'c2a3a53ecc69ae9fc9b13b716965ea78bae7699e35628b9ab7cbaffda5cb4927', N'IU69mSCOwk+Bz1SzrdR6uA==', N'afab453eb8bd4b80aba5f507a6b534c8', N'c2a3a53ecc69ae9fc9b13b716965ea78', N'bae7699e35628b9ab7cbaffda5cb4927', N'2017-03-21 16:02:58.227')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'c2c6598b50bcef80c654fe319d686cb76239c00c0c0c2fcc8a97a6f3e8e1c5c1', N'e/SBC4nR6ke6AE6LjWabxA==', N'943b9ec4664940e8b0733724595e9658', N'c2c6598b50bcef80c654fe319d686cb7', N'6239c00c0c0c2fcc8a97a6f3e8e1c5c1', N'2017-02-16 11:06:24.210')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'c2eba993b468e5242b435b1be09b22f08206f1263c2e9949b34dbe1ea1ce49dd', N'QP9XdN7UUUeNriEewQp9MA==', N'af70b51e0bc24bfb9419f90644cf25d2', N'c2eba993b468e5242b435b1be09b22f0', N'8206f1263c2e9949b34dbe1ea1ce49dd', N'2017-03-27 12:03:50.920')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'c3ffc5fb88318569be88c10c05e0275f97054926e37a17c6157c335e400df7ec', N'lB7C4dyLtUiiKcNfP8ciIg==', N'42c0b1a51add484bb64ebc2a1a72de45', N'c3ffc5fb88318569be88c10c05e0275f', N'97054926e37a17c6157c335e400df7ec', N'2017-01-14 19:14:20.073')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'c4229d2381cea601da9018be1b1e287d70d40b0743d82ce42e1cc837327d8819', N'V/NuUTb5qUC5cO0mGylrkA==', N'ee9c024545d7416a9adccd07f128d785', N'c4229d2381cea601da9018be1b1e287d', N'70d40b0743d82ce42e1cc837327d8819', N'2017-03-01 15:04:35.663')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'c563aa72a85db6499b4605bc05824887e2e000ef0c84315a8d90856b3555a18b', N'qfnT0b3JpUKsQHFgQfNKag==', N'9a70a664165a47a89c883a7dd18a93c5', N'c563aa72a85db6499b4605bc05824887', N'e2e000ef0c84315a8d90856b3555a18b', N'2016-12-27 18:46:12.393')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'c5739c68c565eb86214872a6943233b8f42a74888cc337c7031ad14bec236d40', N'D494r8Kf9ku3vKbGQrVbcw==', N'cd17192e10a24f3fb5e1408061f2b0f2', N'c5739c68c565eb86214872a6943233b8', N'f42a74888cc337c7031ad14bec236d40', N'2017-02-13 23:52:19.140')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'c669d2244e9de1f73f79ffcb7b16b907af70662299e54e8686645ca774e6cf3e', N'SN5C31yIDEebLFiYFAyKvg==', N'0fb00dc7661c4066b24afd6049506985', N'c669d2244e9de1f73f79ffcb7b16b907', N'af70662299e54e8686645ca774e6cf3e', N'2017-02-08 11:00:30.180')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'c6e4e24013ffb991149d9f255745a179c36c6fa52838d1c4859fd541bcacb457', N'T4A+aKGoP0ycMrEbHGKcKw==', N'abb46eeb371a484ebd2ddaf050a97ea7', N'c6e4e24013ffb991149d9f255745a179', N'c36c6fa52838d1c4859fd541bcacb457', N'2017-01-14 19:12:24.620')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'c7073d7ad258668e841550adda10f8d542b0b01dfe1307a19135d8d5b7c29c9b', N'/rZ27CdbnEa3NQvVB7CRQA==', N'c37ac16669fd45c2917799ca5175f233', N'c7073d7ad258668e841550adda10f8d5', N'42b0b01dfe1307a19135d8d5b7c29c9b', N'2017-01-17 17:37:16.310')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'c7d277dc717174a52cc21824196a6ad809c0c7cb8c37825f9edbca7d835c7fac', N'bjgN/xTSGUy8D/fy7EuPYw==', N'51d95112b88342078f524ccba2954c10', N'c7d277dc717174a52cc21824196a6ad8', N'09c0c7cb8c37825f9edbca7d835c7fac', N'2017-02-06 14:57:13.630')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'c81048247c565ca70cc89ec268bd866afaa7e0708072b79518c3e69d24f1dab3', N'G6gTPMLWLkiJdZzWXgylaQ==', N'c0c83741edd2435abb086e3a929d7a67', N'c81048247c565ca70cc89ec268bd866a', N'faa7e0708072b79518c3e69d24f1dab3', N'2017-02-03 12:43:38.150')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'c97d10a29adca67495897e556206a81fb9b7465a5d1f1d365b5a14c125460914', N'Pb2Q4Jnt8EuvRFpGFFLUFA==', N'2565d1d1b8ad46649af5b79c04aa37e7', N'c97d10a29adca67495897e556206a81f', N'b9b7465a5d1f1d365b5a14c125460914', N'2017-03-16 17:40:58.130')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'ca1953656a2001844a9d4a56a2640912b3395c98ae6987ca70a651753fa880e9', N'iUDNNihlUk++TwSXMGqSTw==', N'6cf6ceb287ce4ee9bd992c5b75148efa', N'ca1953656a2001844a9d4a56a2640912', N'b3395c98ae6987ca70a651753fa880e9', N'2017-02-06 15:54:51.870')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'ca8974bb7b06b629d4ec6bd2b982947a587e251acadf958b4f7068852bab765b', N'A6js/4H0BkaJFLeeCii0Zg==', N'99187c494e0443fd8f2927b15282056d', N'ca8974bb7b06b629d4ec6bd2b982947a', N'587e251acadf958b4f7068852bab765b', N'2017-02-12 19:17:43.410')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'cbab710134c13e622ecd94d8bdf544ec42a2ae06526d23e4f5de00cf3f98a690', N'3PwND2fxgkmaid8rbwiF9g==', N'E95A4BF8-B216-4177-B12A-4CCE0C00401D', N'cbab710134c13e622ecd94d8bdf544ec', N'42a2ae06526d23e4f5de00cf3f98a690', N'2017-02-20 12:04:32.543')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'cbfc8f26eb12f87b067560d174e0e02fa1f19611216784b207c15405770e8f3e', N'rGX5SChm2kqDKQKYzFdNyQ==', N'e76a85df6426489897dd3c2f50235711', N'cbfc8f26eb12f87b067560d174e0e02f', N'a1f19611216784b207c15405770e8f3e', N'2016-12-28 10:24:14.140')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'cc9153226b872b3dd420b9403a185a82e1da0f2d2d28ba17b78fe5d1b11f5301', N'yFs/urySMUm5kWfd5KO1GA==', N'ee7a3ca1a38440d8be994edfad5e8cb5', N'cc9153226b872b3dd420b9403a185a82', N'e1da0f2d2d28ba17b78fe5d1b11f5301', N'2017-02-08 15:31:50.243')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'ccb61cb5d74282e04b608326b8f1e95ab3395c98ae6987ca70a651753fa880e9', N'qbiXle2Ick2SSkkO1+jzxg==', N'6cf6ceb287ce4ee9bd992c5b75148efa', N'ccb61cb5d74282e04b608326b8f1e95a', N'b3395c98ae6987ca70a651753fa880e9', N'2017-02-06 17:10:46.597')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'cd246970a8ddb718b78234ba7a6530c5d3167f5470a7883ea64495288a9d2281', N'03o+04obOUay1AvjErsMGQ==', N'1c80a23ef50842da98d50bdc8dd44e95', N'cd246970a8ddb718b78234ba7a6530c5', N'd3167f5470a7883ea64495288a9d2281', N'2017-03-03 15:18:20.263')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'ce0e1bfb45b5277c91c8f29c3fa16d2bf1f4b1c19492ab1b0b73bbfc7d53ac72', N'+M0fg1IBK0uOstzo1r2pWw==', N'0f275c5356a74380835f054237f0fc91', N'ce0e1bfb45b5277c91c8f29c3fa16d2b', N'f1f4b1c19492ab1b0b73bbfc7d53ac72', N'2017-03-23 15:58:47.930')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'ce8a5048f02bf255d419480f084906b51aa55704d96c3f11ef0f6760a0d70f1f', N'9cEf4IrfZ0qxkMW8+UrDuw==', N'6c7c99ed532a4436bc6d1dbc122cdd14', N'ce8a5048f02bf255d419480f084906b5', N'1aa55704d96c3f11ef0f6760a0d70f1f', N'2017-03-18 10:42:13.417')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'cf1772a118756d2af552058180bdd5364c1dc1863f83f8b50e65c2e286c079fa', N'b33WVwJpGkqO+m2/azdhiw==', N'7c24db0a5e574d5bbbe740a979574de2', N'cf1772a118756d2af552058180bdd536', N'4c1dc1863f83f8b50e65c2e286c079fa', N'2017-03-28 10:26:00.697')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'd105ccd7ba73c767a5847069399e377c43eac010e1dca5ed82d6e6d30200d74a', N'wwLo4NVLfkqCiId6Cs+ETQ==', N'cd0665d1890643ed806755bcef7c7734', N'd105ccd7ba73c767a5847069399e377c', N'43eac010e1dca5ed82d6e6d30200d74a', N'2017-03-08 15:02:45.573')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'd108bf0ac76e73cdf2252adf28b03ba1d2e035c7705e238fa33cc1c4c639e85b', N'5gIea7mhLkG/W2IF4jYziA==', N'af07d231ad5c4810bd336fb982864c53', N'd108bf0ac76e73cdf2252adf28b03ba1', N'd2e035c7705e238fa33cc1c4c639e85b', N'2016-12-28 23:28:07.153')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'd11abe931b6fa765ad32dae64839b9cfb3395c98ae6987ca70a651753fa880e9', N'UVkeYkazEkCSJEU96Ms9oA==', N'6cf6ceb287ce4ee9bd992c5b75148efa', N'd11abe931b6fa765ad32dae64839b9cf', N'b3395c98ae6987ca70a651753fa880e9', N'2017-02-07 15:13:05.647')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'd1a6cc295a594fd184ad52090a31e4f93f305ae458361b19d282a74b742eeaef', N'BXrVZl2kFUmwi3Uci313GQ==', N'9e009a2de0aa46b497b6041c4033c8ea', N'd1a6cc295a594fd184ad52090a31e4f9', N'3f305ae458361b19d282a74b742eeaef', N'2017-01-11 15:46:55.707')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'd309b5d85dfeb6203495b7778ee4a25e248abbbd33477b1e54bc659dcf3b2dde', N'W/jkdynfxUKx57bZVVPz2A==', N'7e8aaec017f340b3ae9a0045c49b0230', N'd309b5d85dfeb6203495b7778ee4a25e', N'248abbbd33477b1e54bc659dcf3b2dde', N'2017-03-07 16:16:10.137')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'd346f4b9e2d4908e7e43a903aaa1b032fa197e85cae2211bea6dbca9ccf2769e', N'svlJlKcFBkigNTVmLuETFA==', N'eb0b1d18a7de476fad784613fe435683', N'd346f4b9e2d4908e7e43a903aaa1b032', N'fa197e85cae2211bea6dbca9ccf2769e', N'2017-02-16 20:39:02.023')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'd443a01d0551213e610bd5a1662d42e57a2828448650b120b32b5c5d0929b575', N'JdWVradmWECltXL2yh/dpg==', N'b663695f6e7d4a7d9f2b3fa7b3a674c5', N'd443a01d0551213e610bd5a1662d42e5', N'7a2828448650b120b32b5c5d0929b575', N'2016-12-26 19:25:05.863')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'd4aa266c7130e1f98bc36a812ec258bcec98d559ea22e9f523c71471f367f893', N'kxqp1IBqQE6UsKaRz/nKUQ==', N'63abd37e69fa4330a6510c80ac14e598', N'd4aa266c7130e1f98bc36a812ec258bc', N'ec98d559ea22e9f523c71471f367f893', N'2017-03-06 10:32:09.360')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'd4c8d9afb01d5ac9a00c7a8afc190a09bd0db2373571d56759c8431fb4b1da4e', N'zE4w17XfNUmJPPZzLtCh2w==', N'75cf2d66a9a941d6829f24eac4af76d0', N'd4c8d9afb01d5ac9a00c7a8afc190a09', N'bd0db2373571d56759c8431fb4b1da4e', N'2017-03-08 09:42:13.980')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'd4f84a11655bc212375aedcd5d2202b4a2ddeed26fbf832708b338ff7fb64394', N'fOoDrXiXTU+CVTi5a2XJAQ==', N'35a2fe678b2b4a729171723bd96c9ee0', N'd4f84a11655bc212375aedcd5d2202b4', N'a2ddeed26fbf832708b338ff7fb64394', N'2017-03-18 14:12:30.240')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'd655c6a878f66c47c1b4530facc998dee315ff9db71a8fe999d3f3f70ef02640', N'zQoqhIv1lk+eAm0ZNA+g8g==', N'a61cc7b784d74da8a129d9082bd597f0', N'd655c6a878f66c47c1b4530facc998de', N'e315ff9db71a8fe999d3f3f70ef02640', N'2017-02-08 11:24:49.270')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'd6e96257aceb6e135072ede752b42b85d424a53c8e2bfcbaebc8f746a119a3d7', N'xNg4UAaTs0WPD3yckeRKnw==', N'5dd9afbb5b5f4e35b1c8249fcdaf27d8', N'd6e96257aceb6e135072ede752b42b85', N'd424a53c8e2bfcbaebc8f746a119a3d7', N'2017-03-14 10:06:12.453')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'd73fdc290dcecc08270d3eb91c69f91c93aa0e79c5cbfbddb17733e8795d5a40', N'Tgd9dESj7EiP1i7vwezSiw==', N'156bb15ba425460c93a75577dcb3c742', N'd73fdc290dcecc08270d3eb91c69f91c', N'93aa0e79c5cbfbddb17733e8795d5a40', N'2017-02-15 10:55:59.810')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'd78916452d8bbdc07e7db570b1cda5c4f4b72282c0a4b27f04a955874a40f768', N'b9lANXgYDU2xjdp2pXl4WA==', N'cce257eba0c444c9a5d85aa2219ca14d', N'd78916452d8bbdc07e7db570b1cda5c4', N'f4b72282c0a4b27f04a955874a40f768', N'2017-02-12 15:43:20.457')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'd80ac2a30623eeb2f276711691135b2fe314b7ed925b432df13d650469e8daf5', N'wwzcwIQ1KEOGgGJVpvgxUg==', N'77b24ed55b664760a7b4fe5813572894', N'd80ac2a30623eeb2f276711691135b2f', N'e314b7ed925b432df13d650469e8daf5', N'2017-03-10 15:28:50.693')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'd884422f0ce6b7214df0268fcdcfef4380aef9329159d85f95c049a4db8546b4', N'YJvdHsuYo0uJivjqIythag==', N'c7bdcb27ca6a48b895288981907bdd9f', N'd884422f0ce6b7214df0268fcdcfef43', N'80aef9329159d85f95c049a4db8546b4', N'2017-01-05 14:17:07.727')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'd9301e38a980a3be17ef83fc7b1ba129bd8efed78fa49fe063b2fc8a8d85a869', N'A66U3On2b0Wr41w8iETHBw==', N'0fe1fc8f0ea54cd98d58fde0131c7d0f', N'd9301e38a980a3be17ef83fc7b1ba129', N'bd8efed78fa49fe063b2fc8a8d85a869', N'2017-03-01 11:09:51.573')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'd9634cd79898ea3ed553a150ff47506122e0ae92e2ccea357167229248a4e529', N'xs727qX4F02ez0BPElAgXw==', N'cf47d029385241c797564bf6e793c61d', N'd9634cd79898ea3ed553a150ff475061', N'22e0ae92e2ccea357167229248a4e529', N'2017-02-16 17:03:11.147')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'd9f391f6d5dc3606f9ae3177ed6ad4d1a3cf91cdf52b0def500dff0207d11dd2', N'IfSsJJ78EECFsLWJSb4qOg==', N'e3e22426007e4c4f8059da5bdbd7bec2', N'd9f391f6d5dc3606f9ae3177ed6ad4d1', N'a3cf91cdf52b0def500dff0207d11dd2', N'2017-01-10 15:17:24.097')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'da585f77ac350b97f04475afe8fcb24873b95b07e2b57c89dfedd8d1a4a94466', N'qKahmDGGEkSOVHoXNDwtBA==', N'7d3c2b84fe834f2b83bc1ee295e733fc', N'da585f77ac350b97f04475afe8fcb248', N'73b95b07e2b57c89dfedd8d1a4a94466', N'2017-01-06 16:26:42.820')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'dab6ebf7c0ec86fd06c30e7eed4819c96ba6dbc2dee0e7acd91f0fc4674589e3', N'y3Ghk8PaZkCJl25ywKctNA==', N'963ecf4b23294e8bbb53a45397ba4e22', N'dab6ebf7c0ec86fd06c30e7eed4819c9', N'6ba6dbc2dee0e7acd91f0fc4674589e3', N'2017-02-04 14:55:12.410')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'dabcfcf33cae7f77f1f75cee254946b9d237df8a8e9a33cf1c31ca193e396a86', N'At02tAKdXESsWkFNms1IXg==', N'eaa8e2b6eb3e40a6aa4c7fa703282ce1', N'dabcfcf33cae7f77f1f75cee254946b9', N'd237df8a8e9a33cf1c31ca193e396a86', N'2017-02-10 15:48:34.780')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'db028b651ce9dbe881f1453d8edd819e757685749893bdf04128c9bef6a46ef1', N'WfQdR3iPlEmHT/lhA4BTIQ==', N'ca43fcbad36c4ee3b9fe916dbe2f4a68', N'db028b651ce9dbe881f1453d8edd819e', N'757685749893bdf04128c9bef6a46ef1', N'2017-01-15 11:41:08.103')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'dc384066e1f1373b085c7958ac52ea03621807944f68fa3088688ccfe2d323a7', N'zWgOTKglOEuQUWgJmd482Q==', N'154b408ff07b499f8a8859c4e1b4d7c1', N'dc384066e1f1373b085c7958ac52ea03', N'621807944f68fa3088688ccfe2d323a7', N'2017-03-20 18:08:31.327')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'de32f4a32d7a2fa2008ee81d9d2dbb6ded84964f3016dda748ac8260bd6fdb4b', N'nDIZpg70cUuvtRDCAYRU+g==', N'a17e5c625318447085602f7036d3a023', N'de32f4a32d7a2fa2008ee81d9d2dbb6d', N'ed84964f3016dda748ac8260bd6fdb4b', N'2016-12-23 18:07:28.983')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'df52b1d6f1fea2e459df75569f7b8f35675b2fbde2a327ec1222d8245ea1a712', N'znk3wJ/6x0iGnE/mAdi/zg==', N'5275175d92ea487080eb80b8dc8a125c', N'df52b1d6f1fea2e459df75569f7b8f35', N'675b2fbde2a327ec1222d8245ea1a712', N'2017-03-07 09:59:33.280')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'e0996829ce46c0fe81a053bbbf925afaa523c0bcf7ac1958a51f3e2f656ed9bb', N'TDipLgtmv0O4L+YK5zX4TQ==', N'57466145692f423a93ea5b37a651ca7a', N'e0996829ce46c0fe81a053bbbf925afa', N'a523c0bcf7ac1958a51f3e2f656ed9bb', N'2017-02-15 16:33:38.110')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'e0de25075427e0b178adac4c131f84872c6257c1c6a5e557b59c50f6417e307d', N's+Jbs8/2H0ql6sovQDMa8g==', N'61f9a9520f51453eab54a5c4cfe5db7a', N'e0de25075427e0b178adac4c131f8487', N'2c6257c1c6a5e557b59c50f6417e307d', N'2017-03-15 15:58:05.577')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'e0e895d3a0ced9cf19b1733c0a812285971d711e59ec0e09836fc11894f7fdaa', N'0PdLmftE9U+oBK/pwTt9nA==', N'ac368054fcd44d648e7a079c3c821176', N'e0e895d3a0ced9cf19b1733c0a812285', N'971d711e59ec0e09836fc11894f7fdaa', N'2017-03-14 19:40:03.740')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'e1a8f040eadd042e4c2b0e7f7297c8a3956957899ea7f835f5a58c78be5c2dbe', N'X0u3lUU3yUS0PRwT6UWGQQ==', N'29c4de745f25485fa2bc0b0f9c2be673', N'e1a8f040eadd042e4c2b0e7f7297c8a3', N'956957899ea7f835f5a58c78be5c2dbe', N'2017-03-11 12:00:35.640')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'e1f26ed54200e6edbaf96bddb433dc1240a80d646d640b25e09cea442cc6ded4', N'9e4LyPHB+0a5dAaTpZH39w==', N'49b6c211b6ae49a1880f942d7134e0b2', N'e1f26ed54200e6edbaf96bddb433dc12', N'40a80d646d640b25e09cea442cc6ded4', N'2017-01-23 12:18:41.017')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'e1fa3c97e2202c8d7944710eaf2a4f2120d458a1402717b4a1f7a889bee90661', N'UpMa+huPEUi3EBj2SjBgfg==', N'f6de629ecf3a403187e24d0763d3951e', N'e1fa3c97e2202c8d7944710eaf2a4f21', N'20d458a1402717b4a1f7a889bee90661', N'2017-02-13 09:51:30.723')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'e2a0df1fdac7b705475179089d051d005a825816a3840e49e1d22af85a89bf4d', N'lHIVmri2xEaMHF1fNE1tcg==', N'526896e007c64bbe90f60b9e299bd89b', N'e2a0df1fdac7b705475179089d051d00', N'5a825816a3840e49e1d22af85a89bf4d', N'2017-03-09 18:04:50.177')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'e32a68e8c385d65b6e3b009ae3b99bfc7572a47a6efe60a3314024606e979650', N'mYDAK6P12kCa7bCU+N4IHQ==', N'a21e74abd6af4f398a58619d64b27508', N'e32a68e8c385d65b6e3b009ae3b99bfc', N'7572a47a6efe60a3314024606e979650', N'2017-01-11 16:31:10.420')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'e3fb5382f18580faf1949dad53bb714db3395c98ae6987ca70a651753fa880e9', N'xa5Na0iWq0iSETfsUS0EOA==', N'6cf6ceb287ce4ee9bd992c5b75148efa', N'e3fb5382f18580faf1949dad53bb714d', N'b3395c98ae6987ca70a651753fa880e9', N'2017-02-06 17:10:18.670')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'e3fed5407ce7c87110a227fba50b1217e36293c03087d369265c6991817e1ff4', N'jd3Ia2AaT0yd6k+VT80TmA==', N'e5ee4a2f1d2249d4af2bb9a8a5c9ab6a', N'e3fed5407ce7c87110a227fba50b1217', N'e36293c03087d369265c6991817e1ff4', N'2017-02-06 15:38:53.527')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'e42559291d78dd9d23511c0e6be4ab0ba65e39566f302a66bbc490a10103c090', N'bk/48M3rgkm7fm/t6MBaqg==', N'1b40bf491f0a4319b5e6e036bf392233', N'e42559291d78dd9d23511c0e6be4ab0b', N'a65e39566f302a66bbc490a10103c090', N'2017-01-23 10:21:23.173')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'e46ee92b32721f6beae97a06483d0736c827d75c1afc782509103bdc0ab43358', N'4WE9bq1DxE2HEk82zQ/K1g==', N'8739f28e4dc240bea52d01c092e9ad3b', N'e46ee92b32721f6beae97a06483d0736', N'c827d75c1afc782509103bdc0ab43358', N'2017-03-23 15:23:29.100')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'e4847b31dc7584cd0355acc2bf9aa14836f2e3e454f7ed2cebc43bfe30d8d46b', N'i+YfV/yVLEam+ZxFeeKfKQ==', N'39a20b53732940739a6cc460648db941', N'e4847b31dc7584cd0355acc2bf9aa148', N'36f2e3e454f7ed2cebc43bfe30d8d46b', N'2017-03-17 19:25:28.400')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'e4bc403656c55441dd92edc3d10e651436511c638c01b4333e4256ec9e7734e1', N'nFMOXyQGUkSrN/I0MblMvA==', N'c4f4596d937244e18f4a284a38c4266c', N'e4bc403656c55441dd92edc3d10e6514', N'36511c638c01b4333e4256ec9e7734e1', N'2017-02-06 15:17:25.047')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'e519efa5d73f0e59212818251f83ea0f9f9d4b54cc49eccdb527d63c7203cac5', N'hIvXvWZbXEmLt1289DwAFQ==', N'fca5b055805e46189a4598c622bb570b', N'e519efa5d73f0e59212818251f83ea0f', N'9f9d4b54cc49eccdb527d63c7203cac5', N'2017-02-23 13:36:06.770')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'e57477e3beea5ead3dbb541cea70601f0fae116c6ad948cbfe81e7060870566d', N'4L6gGZAgQEGrCd240RcBYw==', N'5d3a398b82af429f92b707e16634d053', N'e57477e3beea5ead3dbb541cea70601f', N'0fae116c6ad948cbfe81e7060870566d', N'2016-12-22 11:24:17.583')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'e6215e380d0cfeeaf0b56be7274cb038fe64595b7972a2831d1a4a54fdf3b0f0', N'RoXIWYATV0Gt150T7SAuxQ==', N'0122f56bba064d8f940f321dda103047', N'e6215e380d0cfeeaf0b56be7274cb038', N'fe64595b7972a2831d1a4a54fdf3b0f0', N'2017-03-22 10:16:49.430')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'e75da70d77581a14f509e011deea3067586c8e20c714c6463553736b856c2635', N'JgnS0cHEo0aFnatIXSFg1w==', N'f501fc1e539e46b2846e07e4c3817f1d', N'e75da70d77581a14f509e011deea3067', N'586c8e20c714c6463553736b856c2635', N'2017-01-17 14:43:50.660')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'e75fb68cdb5f5e9de7ac9b9a374dd90ac1a0d4499d5c1c1fc12163d9b8d94e3d', N'aC2fAaVKEE6FyQ9gtAumyA==', N'6afaaf3f980d474c913d72fec93a0adc', N'e75fb68cdb5f5e9de7ac9b9a374dd90a', N'c1a0d4499d5c1c1fc12163d9b8d94e3d', N'2017-02-07 19:01:37.743')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'e8549977d1ff125755b7b1e3d3e43312a79abee6318a183b934691766639e2e2', N'xmN45gNjz0ObE8srr5FIFg==', N'0bc8e688c0f547f4810e874c8ca84f4c', N'e8549977d1ff125755b7b1e3d3e43312', N'a79abee6318a183b934691766639e2e2', N'2016-12-21 21:27:17.367')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'e9d0d5987d582897a4170630b10ed019d365dca7b3d99058c46bda2550bb3824', N'C5SqtAqPE0WH6QfmXL5NOg==', N'd52b6871fdcc4a079f9c41ff42ad0ce6', N'e9d0d5987d582897a4170630b10ed019', N'd365dca7b3d99058c46bda2550bb3824', N'2016-12-30 12:28:19.947')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'ea27c2b414e49ffd012bd0a01c72a2208407ce975b11b073e3c952eaae7eabda', N'aPKswoCs6E+2ipMQSyHhHA==', N'f24b174303c842ccb8297e720a429258', N'ea27c2b414e49ffd012bd0a01c72a220', N'8407ce975b11b073e3c952eaae7eabda', N'2017-02-14 12:09:58.387')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'eac07fa516c7eb8cee783d9db7ae5cc7b422ee5a611814dacf98f950009dfc5c', N'uA/e3Kqs3EGXxmmq97r0lA==', N'39ab2424ec6b40009ae0a9141e578eca', N'eac07fa516c7eb8cee783d9db7ae5cc7', N'b422ee5a611814dacf98f950009dfc5c', N'2017-02-06 14:56:05.103')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'eb45c7e1c84a4973b25e80593c00d1c153759984a9a6cec31a4cb26658fb1e66', N'uK5p+FHMfkKlmlv0pD/qyQ==', N'85a33d8ba6be456d8abf3b3f5efbc8a2', N'eb45c7e1c84a4973b25e80593c00d1c1', N'53759984a9a6cec31a4cb26658fb1e66', N'2016-12-26 22:17:41.587')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'eb95a0ff2a57232bbedce29f40eee49eac05f65edd8547cacf94660be25c404c', N'aFf6Od62sUK1w5Duj/MB1g==', N'996b4222d2d746b0be41cd6658d0b09b', N'eb95a0ff2a57232bbedce29f40eee49e', N'ac05f65edd8547cacf94660be25c404c', N'2017-02-27 10:24:47.393')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'ebff00b192172e0ff8a4882a59673eea47de65cc028d419e5f5860aec49aeb20', N'YtJtZnnNy0eYy+QP+KqEAQ==', N'd28c901e5d844117a8909a6b572514d7', N'ebff00b192172e0ff8a4882a59673eea', N'47de65cc028d419e5f5860aec49aeb20', N'2017-02-18 19:33:05.927')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'ed74ff664437c270da86198c491ecb4b6c8ce48d03e37b82b43990e6712fd2f5', N'hfqeDNjB90a/G7gkrDhMVw==', N'6feeccafae2448f89f01ffbbba8e2791', N'ed74ff664437c270da86198c491ecb4b', N'6c8ce48d03e37b82b43990e6712fd2f5', N'2017-02-14 12:56:03.423')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'eddf3efe219233105c1fa5b44a99c8c800cffd93e3d62c03e67db4c13c451012', N'qUYqc0b/Tky/oue7V56aIQ==', N'636679746b414cc2976eea0fcbf86e61', N'eddf3efe219233105c1fa5b44a99c8c8', N'00cffd93e3d62c03e67db4c13c451012', N'2017-03-21 18:05:27.440')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'ee008e00106b4e99bd4a5737a5d84a8bc21b445ce8363971e8fec7920f8b83bd', N'm26nFcXHPkmXcTuluImr0A==', N'5fc756c6d522494da033bdac7c53760e', N'ee008e00106b4e99bd4a5737a5d84a8b', N'c21b445ce8363971e8fec7920f8b83bd', N'2017-03-17 10:15:47.110')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'eea687f1a2898ec5fc7991bc8fbd1755a9477ba8915f9ef793c0d3e87a3ecb1a', N'Ffu9zWU1ZE6ThTqNBSBj4g==', N'44466c3b61604379a248dbd1877bd415', N'eea687f1a2898ec5fc7991bc8fbd1755', N'a9477ba8915f9ef793c0d3e87a3ecb1a', N'2017-02-20 16:45:22.537')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'efa8955c34e857034086b4039d4386d131640af526752171c646dab42f033ebe', N'aW2OEQOy0U2MAlgV4iHYQA==', N'399617f604b74c3a9517a8319b838d2f', N'efa8955c34e857034086b4039d4386d1', N'31640af526752171c646dab42f033ebe', N'2017-03-22 11:59:29.703')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'f02ec41249b9a244feabe2932a0b4d03df6b91e19e2e3f066acaf56a851dc80d', N'zGkPB/msAEWjv40433hvlg==', N'c752cf38a6a14da2801c4635d9e15b73', N'f02ec41249b9a244feabe2932a0b4d03', N'df6b91e19e2e3f066acaf56a851dc80d', N'2016-12-27 10:11:53.447')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'f1680f2653917cdcbf4c831e92c3de1dbfc388f86b85f84b045a7d1c1263b34d', N'TrSQw8Eqi0+t8qZ3plI+Kw==', N'3957b2b93af34dccbbfa72fdb1a5db2d', N'f1680f2653917cdcbf4c831e92c3de1d', N'bfc388f86b85f84b045a7d1c1263b34d', N'2017-03-22 18:05:24.830')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'f2260761c80b82e716060ef7a522a627e50fc23bd34de0c17fee5bcba6b52f04', N'xF3UOzKX7UackCLndd+cDA==', N'f1bec7d9330f4509bf1096af7a57f4ae', N'f2260761c80b82e716060ef7a522a627', N'e50fc23bd34de0c17fee5bcba6b52f04', N'2017-03-21 10:54:03.693')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'f25f87c96ea264449a7bf00be0644aa73fc4c2d0ef28798ed10c87a2246f00f3', N'nlDeiUNxNEKHMAZjhk1q9Q==', N'8cbee712db5341929a91c0c9748f65e2', N'f25f87c96ea264449a7bf00be0644aa7', N'3fc4c2d0ef28798ed10c87a2246f00f3', N'2017-01-12 15:35:38.720')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'f414e6da448cc2d6a7032f46dc14424fa1a1bcf62dd163ad51b405865b519790', N'V0vNXwj1I0S/Q7SGmgykzw==', N'f68ff2c1cd0d4a79b207707b4456afd3', N'f414e6da448cc2d6a7032f46dc14424f', N'a1a1bcf62dd163ad51b405865b519790', N'2017-01-17 12:37:06.393')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'f52e00187653f8f70a30e641c362983725afb021c9401475a002e9c685b58375', N'OudNMskAO0aPd/iuheqrzw==', N'945b354ebdf04a6caae23a0351bdbc20', N'f52e00187653f8f70a30e641c3629837', N'25afb021c9401475a002e9c685b58375', N'2017-02-23 11:09:15.040')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'f5a5484b2074c0e943a06afc7071b53e19ec8aed7113da767528b5d435d754fe', N'gfTH31TM/UaU0JkJk1xSFg==', N'118b2a6de3154f34a4464201addb0058', N'f5a5484b2074c0e943a06afc7071b53e', N'19ec8aed7113da767528b5d435d754fe', N'2017-03-08 16:36:06.873')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'f5b91ff376c6d148960fb0bf405c46cee9ff1f368971ce0efbb00eaf8c53ab43', N'ceRjEjdk7EWWZiYff/77NQ==', N'b7a9bb642d124829a0ed9a2c417da55e', N'f5b91ff376c6d148960fb0bf405c46ce', N'e9ff1f368971ce0efbb00eaf8c53ab43', N'2017-01-23 12:20:07.580')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'f62788167d3a5932b179310e33b22aa8df7b3c7821cf28fdba957132a673ccfb', N'OqKqYIt7wECvWpaNbHlXEA==', N'6b92eac34ec5435c9c49bd077443e89a', N'f62788167d3a5932b179310e33b22aa8', N'df7b3c7821cf28fdba957132a673ccfb', N'2017-03-15 11:57:15.460')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'f6bbf966b22a17efc4ae7984622e73216529c59845ef753a9ea22fb489296aad', N'0BJL0YPPEU2Gs8Wz2bh/+w==', N'fa0c30c3b91647cea7cb43f667e56da2', N'f6bbf966b22a17efc4ae7984622e7321', N'6529c59845ef753a9ea22fb489296aad', N'2017-01-18 14:47:19.390')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'f6d16498010a89fd16209a096b98391652583d09060e5f3a26d06a1fc9d50b10', N'UOre2h586kKBevR0hngxUA==', N'7f024e424ded4d56b302716042143aa2', N'f6d16498010a89fd16209a096b983916', N'52583d09060e5f3a26d06a1fc9d50b10', N'2017-01-03 09:22:48.463')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'f741ee9ced0d2e3159e44502eaa724688574b5d83086c094e5873c3a17a321f1', N'GNUi/rEFW0a0tFOUOVjCKg==', N'75d7e319a4cc4ca9957f9642718560e2', N'f741ee9ced0d2e3159e44502eaa72468', N'8574b5d83086c094e5873c3a17a321f1', N'2017-01-05 16:58:15.950')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'f7b39a96e8bbeb602dea4c97ceb02658231b7e075e8bd9c6fafb63880d6327c4', N'BluypH8TCEyQdUG+b1rU4w==', N'fb5a2df773eb46d999e4bff06461c70c', N'f7b39a96e8bbeb602dea4c97ceb02658', N'231b7e075e8bd9c6fafb63880d6327c4', N'2016-12-28 17:40:19.867')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'f7e0d2ffa90d186c7829e66db77f020e2f83473402d197873cd6fe62a01ce4fa', N'E+SAD6H9ik+b6xarcpsA7g==', N'c9304d62f5e54bd39877c0203428254f', N'f7e0d2ffa90d186c7829e66db77f020e', N'2f83473402d197873cd6fe62a01ce4fa', N'2017-03-21 15:11:08.900')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'f89f86dbf307083cc7005f2540209d4242721732f919c5a8755621f88035bf45', N'CqYDiajWwEujeixtCkH9/A==', N'ec8901166c1b469c8c7d5d9d29415dbe', N'f89f86dbf307083cc7005f2540209d42', N'42721732f919c5a8755621f88035bf45', N'2017-03-08 11:17:35.820')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'f94e4aba6931425ff4aa55af71eb7e2394c6c16d2d8c6a27e494702a08da2ce8', N'QLAx6JKUKEC2xcNZwSUwxg==', N'bfc7a512b72340c1af9b643df7299d78', N'f94e4aba6931425ff4aa55af71eb7e23', N'94c6c16d2d8c6a27e494702a08da2ce8', N'2017-03-01 13:11:15.600')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'f97dd9e697cb9fb2b4d8791cf1b0268b8792580263f05194fdfefb48ef0bcfe1', N'rd8s02VE4kC6pNLfNn0T9w==', N'343de562cccc4a80a5182e35a448c1cb', N'f97dd9e697cb9fb2b4d8791cf1b0268b', N'8792580263f05194fdfefb48ef0bcfe1', N'2017-03-07 16:51:55.197')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'f9973fa2ac2ddcd6c924f99e87f005ef5813ed5893025d95576823e35c0cdceb', N'CyfoGpK3IkKIzotNVHwC6w==', N'7c344e92a5c44857ace8408faeee7f0b', N'f9973fa2ac2ddcd6c924f99e87f005ef', N'5813ed5893025d95576823e35c0cdceb', N'2017-03-22 13:37:05.933')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'fa413ee906f470a9c6a749f39d478c2b8022e006b1018950ce5d5b99509898ef', N'0bBHflU5s0eXGx2ZaGzq2g==', N'c2d53e01b62a46bdb1a5b17915ea8778', N'fa413ee906f470a9c6a749f39d478c2b', N'8022e006b1018950ce5d5b99509898ef', N'2017-02-20 18:44:44.657')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'fa9b5398607a059c97a6135106a82a550cc9fddaa41a108b597ecbf7f974a022', N'Opj29EeAnEaYst5tVW2STg==', N'34ff5567eba747488da3b84f61d27ede', N'fa9b5398607a059c97a6135106a82a55', N'0cc9fddaa41a108b597ecbf7f974a022', N'2016-12-21 19:30:24.743')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'fa9fbc62b1a6274017dc40d3a185613cb75fe29c2b0b529388e580ae718830d7', N'KpAW0lzIxk+am0zKiY7W8A==', N'592ab5314d16492890fdfe6af336b870', N'fa9fbc62b1a6274017dc40d3a185613c', N'b75fe29c2b0b529388e580ae718830d7', N'2017-03-17 08:35:46.773')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'faee32021cb588fd35d79399def8474777dd1829857c1877c903248c3582a73f', N'WgmOyAbOIUeCONc9yedEqg==', N'25086be74702431d8e5f3b770325ea85', N'faee32021cb588fd35d79399def84747', N'77dd1829857c1877c903248c3582a73f', N'2017-03-21 16:24:29.697')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'faf9bfd34de91d9be3597ec48c32ea1a9e20a633243d2c4fa62319a903aad4e9', N'0cVGuO4XFECjON0XKDv2+A==', N'233a734ff1a74e01a23d5fc82509192c', N'faf9bfd34de91d9be3597ec48c32ea1a', N'9e20a633243d2c4fa62319a903aad4e9', N'2017-01-23 15:03:54.797')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'fb21bd4775dfe47b3ec87688a9add93433c8e553bf8d9df0d24a19715dd39435', N'pOi8mo9h+kCxwfPDUICBwA==', N'0221fb8b655b40dda6e1b62df78cd45a', N'fb21bd4775dfe47b3ec87688a9add934', N'33c8e553bf8d9df0d24a19715dd39435', N'2016-12-28 15:32:37.960')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'fb7ead017fafe978f5b3dc9c9b8a4da7c2f922d2b9b972fddeab7b1e081564ef', N'NlndwBudKEyZrM5nM3kdNA==', N'889d43077451475a9733386abe9eed18', N'fb7ead017fafe978f5b3dc9c9b8a4da7', N'c2f922d2b9b972fddeab7b1e081564ef', N'2017-03-24 16:06:45.543')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'fbbdd8eb72a2f4776cc93da8d8fc8f2c77f14eff10a5e6c27138270ac14acfe1', N'iltPbdiFW0KwUIuDtB4U5g==', N'4d1894afa0b444af85de9f30e50be1dd', N'fbbdd8eb72a2f4776cc93da8d8fc8f2c', N'77f14eff10a5e6c27138270ac14acfe1', N'2017-02-23 18:18:26.753')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'fbcd7df0e5c925132de4ab956d72e8636176e6897314f1bdda3dbca66a763f4b', N'vER5iM8D6U6+k1+AiPc8FA==', N'c400dfdcfe824800b89e8ff91d1a61a3', N'fbcd7df0e5c925132de4ab956d72e863', N'6176e6897314f1bdda3dbca66a763f4b', N'2017-01-19 09:54:12.110')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'fbf25a096d6e099fd32885d2cae9de636102797836c222ba7ce05dad5ccdc1a0', N'QDfYdcmd5EKla/qDhEuzAA==', N'eb60c2e574d24701b72b0587559cdd51', N'fbf25a096d6e099fd32885d2cae9de63', N'6102797836c222ba7ce05dad5ccdc1a0', N'2017-03-01 11:13:23.527')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'fce8e3b85f993bf3f26033e6de088e450623880b389e4619ffa2f0a335cade9a', N'0wo8AL/XE0Gui9q69afzOg==', N'27a0e5729a16475792949a549c00b772', N'fce8e3b85f993bf3f26033e6de088e45', N'0623880b389e4619ffa2f0a335cade9a', N'2016-12-30 16:30:13.300')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'fcecf476d933ff476d394fd0d36249e107c31ab4ea666393d12799510805112b', N'vrDjjFBSmkmMBQZcTFXvmg==', N'ec377688f849429fb4d8cfab9084cc9f', N'fcecf476d933ff476d394fd0d36249e1', N'07c31ab4ea666393d12799510805112b', N'2017-03-17 15:10:52.950')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'fcfdb02c18070ee432a7bca1d4af34c7589cebe51a1c3d46eed60ce56a55e045', N'cRbBpzLYg0WSdro1blRWbg==', N'a826a4b01bf147e3a2efc0fb4fa2ed9d', N'fcfdb02c18070ee432a7bca1d4af34c7', N'589cebe51a1c3d46eed60ce56a55e045', N'2017-02-23 11:02:39.670')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'fd8be42b7cbd04cf4c359255e15397ae4904faf9d03a648d1b4ef360725f0a98', N'B/J9kcIpd0u8ptF0xvKjHg==', N'98546109b4874d6e8caeae41bbc9f882', N'fd8be42b7cbd04cf4c359255e15397ae', N'4904faf9d03a648d1b4ef360725f0a98', N'2017-02-09 19:06:08.997')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'fdcb786664a136b58853981a5bd59b23b7e8c7ac1fda9eb5311db85e0dfeb70e', N'EJedMy4bRky+KTt1ujNLig==', N'6554512becce4827a3140ade1fdc37cc', N'fdcb786664a136b58853981a5bd59b23', N'b7e8c7ac1fda9eb5311db85e0dfeb70e', N'2017-02-13 15:32:51.303')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'fe486dc9d3aaca697abebe24cd1b51abe8b4d77ec344f8d7cbf8a8a0a4c96ed9', N'NV99guzChE6GPBmOj5gHwA==', N'08d77d0fcc3e4bf8bce973e53a0ca410', N'fe486dc9d3aaca697abebe24cd1b51ab', N'e8b4d77ec344f8d7cbf8a8a0a4c96ed9', N'2017-01-17 16:27:50.993')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'fe7b9a544c221ae449979665987bf26aee7b00894406e8a400eb89eea2a0321f', N'MWvTfnv0lUmzx3ej3LLDVw==', N'e1c6de7556c64b7e821512668bc9e866', N'fe7b9a544c221ae449979665987bf26a', N'ee7b00894406e8a400eb89eea2a0321f', N'2017-02-06 10:58:56.820')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'feba3705a19647ea0571cb67d2e4aacfdac8e021ae7f4aa6f5e6daa3f9cda32d', N'mofP1bR7b0STjb4w/B+dPA==', N'cff4705cb48e497b9ffc0e0043509423', N'feba3705a19647ea0571cb67d2e4aacf', N'dac8e021ae7f4aa6f5e6daa3f9cda32d', N'2017-03-23 13:52:51.080')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'ff830f736d1d562c23ebceac7bf8041e6ead9ea3245b3c9bbfc4ec21666fc27f', N'zwYWM+utSkaJMTzpcnXLNA==', N'73e8ef759332431094fb25ade38e4d5e', N'ff830f736d1d562c23ebceac7bf8041e', N'6ead9ea3245b3c9bbfc4ec21666fc27f', N'2016-12-30 14:02:50.023')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'ffbd8f322ae74374e950c70d4f6ac168fd9de7e4ac452a7e54758506b6952e7e', N'eFpA+D7JW0WtMV+NoN5f/g==', N'da4973a37dd04fc8be6914c131770f3c', N'ffbd8f322ae74374e950c70d4f6ac168', N'fd9de7e4ac452a7e54758506b6952e7e', N'2017-02-20 19:12:34.690')
GO
GO
INSERT INTO [dbo].[user_guest_auth_id] ([auth_md5_id], [server_auth_token], [client_auth_token], [server_auth_md5], [client_auth_md5], [reg_date]) VALUES (N'ffc5daf86edf88a239c63048f81837b08181a92b7ff227c71f5c22b57a5d22c6', N'GWeZjAmQb0W5J3nOmz5vEQ==', N'e43424cb1fad4aa9a98c6cc16bd2c81f', N'ffc5daf86edf88a239c63048f81837b0', N'8181a92b7ff227c71f5c22b57a5d22c6', N'2017-03-18 14:10:59.853')
GO
GO

-- ----------------------------
-- Table structure for user_guest_register
-- ----------------------------
DROP TABLE [dbo].[user_guest_register]
GO
CREATE TABLE [dbo].[user_guest_register] (
[regist_idx] bigint NOT NULL IDENTITY(1,1) ,
[before_user_id] bigint NOT NULL ,
[after_user_id] bigint NOT NULL ,
[reg_date] datetime NOT NULL 
)


GO

-- ----------------------------
-- Records of user_guest_register
-- ----------------------------
SET IDENTITY_INSERT [dbo].[user_guest_register] ON
GO
SET IDENTITY_INSERT [dbo].[user_guest_register] OFF
GO

-- ----------------------------
-- Table structure for user_play_game
-- ----------------------------
DROP TABLE [dbo].[user_play_game]
GO
CREATE TABLE [dbo].[user_play_game] (
[user_id] bigint NOT NULL ,
[game_service_id] bigint NOT NULL ,
[reg_date] datetime NOT NULL 
)


GO

-- ----------------------------
-- Records of user_play_game
-- ----------------------------

-- ----------------------------
-- Table structure for user_push_token
-- ----------------------------
DROP TABLE [dbo].[user_push_token]
GO
CREATE TABLE [dbo].[user_push_token] (
[token_idx] bigint NOT NULL IDENTITY(1,1) ,
[user_id] bigint NOT NULL ,
[game_service_id] bigint NOT NULL ,
[service_type] int NOT NULL ,
[push_token] nvarchar(256) NOT NULL ,
[push_status] tinyint NOT NULL ,
[reg_date] datetime NOT NULL 
)


GO

-- ----------------------------
-- Records of user_push_token
-- ----------------------------
SET IDENTITY_INSERT [dbo].[user_push_token] ON
GO
SET IDENTITY_INSERT [dbo].[user_push_token] OFF
GO

-- ----------------------------
-- Table structure for user_push_token_back_20161013_1728
-- ----------------------------
DROP TABLE [dbo].[user_push_token_back_20161013_1728]
GO
CREATE TABLE [dbo].[user_push_token_back_20161013_1728] (
[token_idx] bigint NOT NULL IDENTITY(1,1) ,
[user_id] bigint NOT NULL ,
[game_service_id] bigint NOT NULL ,
[service_type] int NOT NULL ,
[push_token] nvarchar(256) NOT NULL ,
[push_status] tinyint NOT NULL ,
[reg_date] datetime NOT NULL 
)


GO

-- ----------------------------
-- Records of user_push_token_back_20161013_1728
-- ----------------------------
SET IDENTITY_INSERT [dbo].[user_push_token_back_20161013_1728] ON
GO
SET IDENTITY_INSERT [dbo].[user_push_token_back_20161013_1728] OFF
GO

-- ----------------------------
-- Procedure structure for Get_PushTokenList
-- ----------------------------
DROP PROCEDURE [dbo].[Get_PushTokenList]
GO
-- =============================================
-- Author:		manstar
-- Create date: 2016/07/26
-- Description:	get push token list 
-- =============================================
CREATE PROCEDURE [dbo].[Get_PushTokenList]
@game_service_id	BIGINT,
@pageCount		INT = 0,
@getCount 		INT = 10000
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @tryCount INT = 0, @lastidx bigint = 0;
	WHILE(@tryCount < @pageCount)
	BEGIN

	SELECT TOP 1 @lastidx = t.token_idx FROM (
	    SELECT TOP (@getCount) token_idx 
	        FROM dbo.user_push_token upt WITH(INDEX([IX_game_service])) 
	            WHERE upt.game_service_id = @game_service_id AND upt.token_idx > @lastidx AND upt.push_status = 0
	        ORDER BY upt.token_idx ASC
	    ) AS t ORDER BY t.token_idx DESC
	    SET @tryCount += 1;
	END

	SELECT TOP (@getCount)
	    token_idx, user_id, service_type, push_token FROM dbo.user_push_token
	WITH(INDEX([IX_game_service]))
	    WHERE game_service_id = @game_service_id AND push_status = 0
	        AND token_idx > @lastidx
	ORDER BY token_idx ASC
SET NOCOUNT OFF;
END

GO

-- ----------------------------
-- Procedure structure for Get_RandomString
-- ----------------------------
DROP PROCEDURE [dbo].[Get_RandomString]
GO
-- =============================================
-- Author:		manstar
-- Create date: 2016/08/08
-- Description:	get random string for temp pw
-- =============================================
CREATE PROCEDURE [dbo].[Get_RandomString]
@LEN			INT = 8,
@RANDOM_STRING  nvarchar(256) OUTPUT
AS
BEGIN
SET NOCOUNT ON;
	DECLARE @I			INT = 1
	SET @RANDOM_STRING = ''
	
	IF(@LEN > 256)
		SET @LEN = 256

	WHILE @I <= @LEN
	BEGIN
		SELECT @RANDOM_STRING = @RANDOM_STRING + CONVERT(CHAR(1), CASE WHEN CONVERT(INT, (RAND() * 10000)) % 2 = 0 THEN CHAR(RAND() * (57 - 48 + 1) + 48)
															   ELSE CHAR(RAND() * (90 - 65 + 1) + 65)
															   END
										 )
		SET @I = @I + 1
	END	
SET NOCOUNT OFF;
END

GO

-- ----------------------------
-- Procedure structure for System_Get_UID
-- ----------------------------
DROP PROCEDURE [dbo].[System_Get_UID]
GO
-- =============================================
-- Author:		manstar
-- Create date: 2016/07/05
-- Description:	request platform uid (create new or find index)
-- =============================================
CREATE PROCEDURE [dbo].[System_Get_UID] 
	@platform_type int = -1, 
	@platform_user_id nvarchar(128) = '',
	@user_account_status int = 0,
	@ret_result bigint = -1 OUTPUT,
	@ret_platform_user_id nvarchar(128) = '' OUTPUT
AS
BEGIN
	SET NOCOUNT ON
	
	IF(@platform_type < 0 OR LEN(@platform_user_id) < 1 )
	BEGIN
		SET @ret_result = -1
		RETURN @ret_result
	END

	BEGIN TRAN
	BEGIN TRY
		IF EXISTS (SELECT [user_id] FROM [dbo].[user_account] WHERE [platform_type] = @platform_type AND [platform_user_id] = @platform_user_id)
			BEGIN
				SELECT @ret_result = [user_id], @ret_platform_user_id = [platform_user_id]
					FROM [dbo].[user_account] WHERE [platform_type] = @platform_type AND [platform_user_id] = @platform_user_id
				
				COMMIT
				RETURN @ret_result
			END
		ELSE
			BEGIN
				INSERT INTO [dbo].[user_account]
						   ([platform_type]
						   ,[platform_user_id]
						   ,[user_account_status]
						   ,[reg_date])
					 VALUES
						   (@platform_type
						   ,@platform_user_id
						   ,@user_account_status
						   ,GETDATE())
				SELECT @ret_result = SCOPE_IDENTITY()
				SET @ret_platform_user_id = @platform_user_id
				COMMIT
				RETURN @ret_result
			END
	END TRY
	BEGIN CATCH
		SET @ret_result = ERROR_NUMBER() * -1;
		ROLLBACK
		RETURN @ret_result	
	END CATCH
	
	SET NOCOUNT OFF
END

GO

-- ----------------------------
-- Procedure structure for System_Insert_Billing_Info
-- ----------------------------
DROP PROCEDURE [dbo].[System_Insert_Billing_Info]
GO
-- =============================================
-- Author:		manstar
-- Create date: 2016/07/05
-- Description:	insert billing info
-- =============================================
CREATE PROCEDURE [dbo].[System_Insert_Billing_Info]
@user_id		BIGINT,
@game_service_id	BIGINT,
@product_id		INT = 0,
@billing_authkey	NVARCHAR(256),
@billing_token		NVARCHAR(2048),
@billing_platform_type	tinyint,
@billing_status		INT,
@ret_result bigint = -1 OUTPUT
AS
BEGIN
SET NOCOUNT ON;
	DECLARE @price_value INT, @price_tier INT
	SELECT @price_value = [price_value] , @price_tier = [price_tier] FROM [dbo].[game_service_product_id] WHERE [game_service_id] = @game_service_id AND [product_id] = @product_id AND [billing_platform_type] = @billing_platform_type 
	IF @price_value IS NOT NULL
	BEGIN
		BEGIN TRY
			INSERT INTO [dbo].[user_billing_list] ([user_id], [game_service_id], [product_id], [price_value], [price_tier], [billing_authkey], [billing_token], [billing_platform_type], [billing_status], [regdate])
						VALUES					(@user_id,@game_service_id,@product_id,@price_value,@price_tier, @billing_authkey, @billing_token, @billing_platform_type, @billing_status, GETDATE());
			SET @ret_result	= 0
			RETURN @ret_result
		END TRY
		BEGIN CATCH
			SET @ret_result = ERROR_NUMBER() * -1;		-- DB error code
			RETURN @ret_result	
		END CATCH					
	END
	ELSE
	BEGIN
		SET @ret_result = -1
		RETURN @ret_result			
	END
SET NOCOUNT OFF;
END

GO

-- ----------------------------
-- Procedure structure for System_PlatformID_Update
-- ----------------------------
DROP PROCEDURE [dbo].[System_PlatformID_Update]
GO
-- =============================================
-- Author:		manstar
-- Create date: 2016/07/05
-- Description:	request update platform_id
-- =============================================
CREATE PROCEDURE [dbo].[System_PlatformID_Update] 
	@user_id bigint, 
	@platform_type int,
	@platform_user_id nvarchar(128),
	@ret_result int = -1 OUTPUT
AS
BEGIN
	SET NOCOUNT ON
	IF EXISTS (SELECT [user_id] FROM [dbo].[user_account] WHERE user_id = @user_id)
		BEGIN
			UPDATE [dbo].[user_account] SET [platform_user_id] = @platform_user_id, [platform_type] = @platform_type WHERE [user_id] = @user_id;
			SET @ret_result = 0;
		END
	ELSE
		BEGIN
			SET @ret_result = -1;
		END
	SET NOCOUNT OFF
	RETURN @ret_result
END

GO

-- ----------------------------
-- Procedure structure for System_Reg_PlayGame
-- ----------------------------
DROP PROCEDURE [dbo].[System_Reg_PlayGame]
GO
-- =============================================
-- Author:		manstar
-- Create date: 2016/07/05
-- Description:	register UID to play game service  (add new or find index)
-- =============================================
CREATE PROCEDURE [dbo].[System_Reg_PlayGame] 
	@user_id bigint,
	@game_service_id bigint,	
	@ADD_FLAG tinyint = 0,
	@ret_result bigint = -1 OUTPUT
AS
BEGIN
	IF(@user_id <= 0)
	BEGIN
		SET @ret_result = -1
		RETURN @ret_result
	END

	BEGIN TRY
	BEGIN
		IF NOT EXISTS (SELECT [user_id] FROM [user_play_game] WHERE [user_id] = @user_id AND [game_service_id] = @game_service_id)
			BEGIN
				INSERT INTO [dbo].[user_play_game]
						   ([user_id]
						   ,[game_service_id]
						   ,[reg_date])
					 VALUES
						   (@user_id
						   ,@game_service_id
						   ,GETDATE())
				SET @ret_result = 0	  
				RETURN @ret_result
			END		
	END	
	END TRY
	BEGIN CATCH
		SET @ret_result = ERROR_NUMBER() * -1;		-- DB error code
		RETURN @ret_result	
	END CATCH
END

GO

-- ----------------------------
-- Indexes structure for table game_access_auth
-- ----------------------------

-- ----------------------------
-- Primary Key structure for table game_access_auth
-- ----------------------------
ALTER TABLE [dbo].[game_access_auth] ADD PRIMARY KEY ([api_access_id])
GO

-- ----------------------------
-- Indexes structure for table game_service
-- ----------------------------

-- ----------------------------
-- Primary Key structure for table game_service
-- ----------------------------
ALTER TABLE [dbo].[game_service] ADD PRIMARY KEY ([game_service_id])
GO

-- ----------------------------
-- Indexes structure for table game_service_info
-- ----------------------------

-- ----------------------------
-- Primary Key structure for table game_service_info
-- ----------------------------
ALTER TABLE [dbo].[game_service_info] ADD PRIMARY KEY ([game_service_id], [service_type])
GO

-- ----------------------------
-- Indexes structure for table game_service_product_id
-- ----------------------------
CREATE INDEX [IX_product_id] ON [dbo].[game_service_product_id]
([game_service_id] ASC, [product_id] ASC, [billing_platform_type] ASC) 
INCLUDE ([product_index], [price_value], [price_tier]) 
GO

-- ----------------------------
-- Primary Key structure for table game_service_product_id
-- ----------------------------
ALTER TABLE [dbo].[game_service_product_id] ADD PRIMARY KEY ([product_index])
GO

-- ----------------------------
-- Indexes structure for table system_error_code
-- ----------------------------

-- ----------------------------
-- Primary Key structure for table system_error_code
-- ----------------------------
ALTER TABLE [dbo].[system_error_code] ADD PRIMARY KEY ([error_code])
GO

-- ----------------------------
-- Uniques structure for table system_error_code
-- ----------------------------
ALTER TABLE [dbo].[system_error_code] ADD UNIQUE ([error_number] ASC)
GO

-- ----------------------------
-- Indexes structure for table system_push_service
-- ----------------------------
CREATE INDEX [IDX_PushDateSearch] ON [dbo].[system_push_service]
([game_service_id] ASC, [send_reserv_date] DESC) 
GO
CREATE INDEX [IDX_PushMsgCheck] ON [dbo].[system_push_service]
([send_reserv_date] DESC, [push_status] DESC) 
GO

-- ----------------------------
-- Primary Key structure for table system_push_service
-- ----------------------------
ALTER TABLE [dbo].[system_push_service] ADD PRIMARY KEY ([game_service_id], [push_id])
GO

-- ----------------------------
-- Indexes structure for table user_account
-- ----------------------------

-- ----------------------------
-- Primary Key structure for table user_account
-- ----------------------------
ALTER TABLE [dbo].[user_account] ADD PRIMARY KEY ([platform_user_id], [platform_type])
GO

-- ----------------------------
-- Uniques structure for table user_account
-- ----------------------------
ALTER TABLE [dbo].[user_account] ADD UNIQUE ([user_id] ASC)
GO

-- ----------------------------
-- Indexes structure for table user_account_restrict
-- ----------------------------

-- ----------------------------
-- Primary Key structure for table user_account_restrict
-- ----------------------------
ALTER TABLE [dbo].[user_account_restrict] ADD PRIMARY KEY ([user_id])
GO

-- ----------------------------
-- Indexes structure for table user_billing_authkey
-- ----------------------------
CREATE INDEX [IDX_user_id_product_id] ON [dbo].[user_billing_authkey]
([user_id] ASC, [product_id] ASC) 
INCLUDE ([billing_authkey], [platform_type], [payload_info], [regdate]) 
GO

-- ----------------------------
-- Primary Key structure for table user_billing_authkey
-- ----------------------------
ALTER TABLE [dbo].[user_billing_authkey] ADD PRIMARY KEY ([billing_authkey])
GO

-- ----------------------------
-- Indexes structure for table user_billing_list
-- ----------------------------
CREATE INDEX [IDX_user_billing_list_status] ON [dbo].[user_billing_list]
([billing_status] ASC) 
GO
CREATE INDEX [IDX_user_billing_list_user_id] ON [dbo].[user_billing_list]
([user_id] ASC, [billing_status] ASC) 
GO

-- ----------------------------
-- Primary Key structure for table user_billing_list
-- ----------------------------
ALTER TABLE [dbo].[user_billing_list] ADD PRIMARY KEY ([billind_idx])
GO

-- ----------------------------
-- Indexes structure for table user_guest_auth_id
-- ----------------------------

-- ----------------------------
-- Primary Key structure for table user_guest_auth_id
-- ----------------------------
ALTER TABLE [dbo].[user_guest_auth_id] ADD PRIMARY KEY ([auth_md5_id])
GO

-- ----------------------------
-- Indexes structure for table user_guest_register
-- ----------------------------

-- ----------------------------
-- Primary Key structure for table user_guest_register
-- ----------------------------
ALTER TABLE [dbo].[user_guest_register] ADD PRIMARY KEY ([regist_idx])
GO

-- ----------------------------
-- Indexes structure for table user_play_game
-- ----------------------------

-- ----------------------------
-- Primary Key structure for table user_play_game
-- ----------------------------
ALTER TABLE [dbo].[user_play_game] ADD PRIMARY KEY ([user_id], [game_service_id])
GO

-- ----------------------------
-- Indexes structure for table user_push_token
-- ----------------------------
CREATE UNIQUE INDEX [IDX_Unique_push_token_with_game_service_id] ON [dbo].[user_push_token]
([game_service_id] ASC, [push_token] ASC) 
WITH (IGNORE_DUP_KEY = ON)
GO
CREATE INDEX [IX_game_service] ON [dbo].[user_push_token]
([game_service_id] ASC, [push_status] ASC) 
GO
CREATE INDEX [IX_user_id] ON [dbo].[user_push_token]
([user_id] ASC, [game_service_id] ASC) 
GO

-- ----------------------------
-- Primary Key structure for table user_push_token
-- ----------------------------
ALTER TABLE [dbo].[user_push_token] ADD PRIMARY KEY ([token_idx])
GO
