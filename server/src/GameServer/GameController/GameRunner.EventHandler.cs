using GameServer.Connection;
using GameServer.GameLogic;

namespace GameServer.GameController;

public partial class GameRunner : IGameRunner
{
    public void HandleAfterMessageReceiveEvent(object? sender, AfterMessageReceiveEventArgs e)
    {
        if (_isRunning == false)
        {
            _logger.Warning($"Game is not running. Ignoring message: {e.Message.MessageType}");
            return;
        }

        _logger.Debug($"Handling message: {e.Message.MessageType}");

        switch (e.Message)
        {
            case PerformAbandonMessage performAbandonMessage:
                if (!_tokenToPlayerId.ContainsKey(performAbandonMessage.Token))
                {
                    _logger.Error($"Player with token \"{performAbandonMessage.Token}\" does not exist.");
                }
                else
                {
                    try
                    {
                        List<(IItem.ItemKind, string)> abandonedSupplies = new()
                        {
                            (IItem.GetItemKind(performAbandonMessage.TargetSupply), performAbandonMessage.TargetSupply)
                        };

                        Game.AllPlayers.Find(p => p.Id == _tokenToPlayerId[performAbandonMessage.Token])?
                        .PlayerAbandon(performAbandonMessage.Numb, abandonedSupplies);
                    }
                    catch (Exception ex)
                    {
                        _logger.Error(
                            $"Failed to perform action \"Abandon\" for player with token {performAbandonMessage.Token}: {ex.Message}"
                        );
                    }
                }
                break;

            case PerformPickUpMessage performPickUpMessage:
                if (!_tokenToPlayerId.ContainsKey(performPickUpMessage.Token))
                {
                    _logger.Error($"Player with token \"{performPickUpMessage.Token}\" does not exist.");
                }
                else
                {
                    try
                    {
                        Game.AllPlayers.Find(p => p.Id == _tokenToPlayerId[performPickUpMessage.Token])?
                        .PlayerPickUp(
                            performPickUpMessage.TargetSupply,
                            new Position(performPickUpMessage.TargetPos.X, performPickUpMessage.TargetPos.Y),
                            performPickUpMessage.Num
                        );
                    }
                    catch (Exception ex)
                    {
                        _logger.Error(
                            $"Failed to perform action \"PickUp\" for player with token {performPickUpMessage.Token}: {ex.Message}"
                        );
                    }
                }
                break;

            case PerformSwitchArmMessage performSwitchArmMessage:
                if (!_tokenToPlayerId.ContainsKey(performSwitchArmMessage.Token))
                {
                    _logger.Error($"Player with token {performSwitchArmMessage.Token} does not exist.");
                }
                else
                {
                    try
                    {
                        Game.AllPlayers.Find(p => p.Id == _tokenToPlayerId[performSwitchArmMessage.Token])?
                        .PlayerSwitchArm(performSwitchArmMessage.TargetFirearm);
                    }
                    catch (Exception ex)
                    {
                        _logger.Error(
                            $"Failed to perform action \"PickUp\" for player with token {performSwitchArmMessage.Token}: {ex.Message}"
                        );
                    }
                }
                break;

            case PerformUseMedicineMessage performUseMedicineMessage:
                if (!_tokenToPlayerId.ContainsKey(performUseMedicineMessage.Token))
                {
                    _logger.Error($"Player with token \"{performUseMedicineMessage.Token}\" does not exist.");
                }
                else
                {
                    try
                    {
                        Game.AllPlayers.Find(p => p.Id == _tokenToPlayerId[performUseMedicineMessage.Token])?
                        .PlayerUseMedicine(performUseMedicineMessage.MedicineName);
                    }
                    catch (Exception ex)
                    {
                        _logger.Error(
                            $"Failed to perform action \"UseMedicine\" for player with token {performUseMedicineMessage.Token}: {ex.Message}"
                        );
                    }
                }
                break;

            case PerformUseGrenadeMessage performUseGrenadeMessage:
                if (!_tokenToPlayerId.ContainsKey(performUseGrenadeMessage.Token))
                {
                    _logger.Error($"Player with token \"{performUseGrenadeMessage.Token}\" does not exist.");
                }
                else
                {
                    try
                    {
                        Game.AllPlayers.Find(p => p.Id == _tokenToPlayerId[performUseGrenadeMessage.Token])?
                        .PlayerUseGrenade(
                            new Position(performUseGrenadeMessage.TargetPos.X, performUseGrenadeMessage.TargetPos.Y)
                        );
                    }
                    catch (Exception ex)
                    {
                        _logger.Error(
                            $"Failed to perform action \"UseGrenade\" for player with token {performUseGrenadeMessage.Token}: {ex.Message}"
                        );
                    }
                }
                break;

            case PerformMoveMessage performMoveMessage:
                if (!_tokenToPlayerId.ContainsKey(performMoveMessage.Token))
                {
                    _logger.Error($"Player with token \"{performMoveMessage.Token}\" does not exist.");
                }
                else
                {
                    try
                    {
                        Game.AllPlayers.Find(p => p.Id == _tokenToPlayerId[performMoveMessage.Token])?
                        .MoveTo(
                            new Position(performMoveMessage.Destination.X, performMoveMessage.Destination.Y)
                        );
                    }
                    catch (Exception ex)
                    {
                        _logger.Error(
                            $"Failed to perform action \"Move\" for player with token {performMoveMessage.Token}: {ex.Message}"
                        );
                    }
                }
                break;

            case PerformStopMessage performStopMessage:
                if (!_tokenToPlayerId.ContainsKey(performStopMessage.Token))
                {
                    _logger.Error($"Player with token \"{performStopMessage.Token}\" does not exist.");
                }
                else
                {
                    try
                    {
                        Game.AllPlayers.Find(p => p.Id == _tokenToPlayerId[performStopMessage.Token])?
                        .Stop();
                    }
                    catch (Exception ex)
                    {
                        _logger.Error(
                            $"Failed to perform action \"Stop\" for player with token {performStopMessage.Token}: {ex.Message}"
                        );
                    }
                }
                break;

            case PerformAttackMessage performAttackMessage:
                if (!_tokenToPlayerId.ContainsKey(performAttackMessage.Token))
                {
                    _logger.Error($"Player with token \"{performAttackMessage.Token}\" does not exist.");
                }
                else
                {
                    try
                    {
                        Game.AllPlayers.Find(p => p.Id == _tokenToPlayerId[performAttackMessage.Token])?
                        .PlayerAttack(
                            new Position(performAttackMessage.TargetPos.X, performAttackMessage.TargetPos.Y)
                        );
                    }
                    catch (Exception ex)
                    {
                        _logger.Error(
                            $"Failed to perform action \"Attack\" for player with token {performAttackMessage.Token}: {ex.Message}"
                        );
                    }
                }
                break;

            case GetPlayerInfoMessage getPlayerInfoMessage:
                break;

            case GetMapMessage getMapMessage:
                break;

            case ChooseOriginMessage chooseOriginMessage:
                if (!_tokenToPlayerId.ContainsKey(chooseOriginMessage.Token))
                {
                    _logger.Information($"Adding player with token \"{chooseOriginMessage.Token}\" to the game.");
                    try
                    {
                        Game.AddPlayer(
                            new Player(
                                _nextPlayerId,
                                Constant.PLAYER_MAXIMUM_HEALTH,
                                Constant.PLAYER_SPEED_PER_TICK,
                                new Position(chooseOriginMessage.OriginPos.X, chooseOriginMessage.OriginPos.Y)
                            )
                        );
                        _tokenToPlayerId[chooseOriginMessage.Token] = _nextPlayerId;
                        _nextPlayerId++;

                        _logger.Information($"Player with token \"{chooseOriginMessage.Token}\" joined the game.");
                    }
                    catch (Exception ex)
                    {
                        _logger.Error(
                            $"Failed to add player with token \"{chooseOriginMessage.Token}\" to the game: {ex.Message}"
                        );
                    }
                }
                else
                {
                    _logger.Error($"Player with token \"{chooseOriginMessage.Token}\" already exists.");
                }
                break;

            default:
                _logger.Error($"Unknown message type: {e.Message.MessageType}");
                break;
        }
    }
}