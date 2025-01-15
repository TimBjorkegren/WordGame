using System;
using System.Data;
using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Npgsql;

namespace app.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ValidateInviteController : ControllerBase
    {
        [HttpPost("validate-invite")]
        public IActionResult ValidateInvite([FromBody] InviteValidationRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.InviteCode))
            {
                return BadRequest("Incorrect, check out for spaces or null");
            }

            try
            {
                var validator = new InviteValidator();
                var isMatch = validator.ExactMatchCode(request.InviteCode);

                if (isMatch)
                {
                    var clientId = GetClientId();
                    Console.WriteLine("HEEEEEEEEEEEEEEEEEEEJ" + clientId);
                    var wasAdded = AddPlayer2ToLobby(request.InviteCode, clientId);

                    if (wasAdded)
                    {
                        return Ok(new { message = "Player 2 has join the lobby" });
                    }
                    else
                    {
                        return Ok(
                            new { message = "Lobby is full or its the incorrect lobby code" }
                        );
                    }
                }
                else
                {
                    return NotFound(new { message = "Invite code does not match." });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in ValidateInvite: {ex.Message}");
                return StatusCode(500, "An error occurred while validating the invite code.");
            }
        }

        [HttpGet]
        [Route("gamestatus/{lobbyId}")]
        public async Task Get(string lobbyId)
        {
            Response.Headers.Add("Content-Type", "text/event-stream");

            while (true)
            {
                if (CheckIfPlayer2Ready(lobbyId))
                {
                    await Response.WriteAsync("event: gameStarted\ndata: \n\n");
                    Console.WriteLine("Finished");
                    break;
                }

                await Task.Delay(2000);
            }
        }

        private string GetClientId()
        {
            if (Request.Cookies.TryGetValue("ClientId", out string clientId))
            {
                Console.WriteLine($"ClientId cookie retrieved: {clientId}");
                return clientId;
            }
            else
            {
                var newClientId = Program.GenerateUniqueClientId();
                Response.Cookies.Append(
                    "ClientId",
                    newClientId,
                    new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true,
                        SameSite = SameSiteMode.None,
                        Expires = DateTimeOffset.UtcNow.AddDays(30),
                    }
                );
                Console.WriteLine($"New ClientId cookie set: {newClientId}");
                return newClientId;
            }
        }

        private bool CheckIfPlayer2Ready(string lobbyId)
        {
            var _dbConnect = new DatabaseConnect();
            var sqlQuery =
                @"SELECT * FROM english_dictionary.lobbys
            WHERE invite_code = @lobbyId AND player2_client IS NOT NULL";

            using var conn = _dbConnect.GetConnection();
            using var cmd = new NpgsqlCommand(sqlQuery, conn);
            cmd.Parameters.AddWithValue("lobbyId", lobbyId);
            using (NpgsqlDataReader reader = cmd.ExecuteReader())
            {
                Console.WriteLine(reader.HasRows);
                return reader.HasRows;
            }
        }

        private bool AddPlayer2ToLobby(string inviteCode, string player2Client)
        {
            var _dbConnect = new DatabaseConnect();
            var sqlQuery =
                @"
        UPDATE english_dictionary.lobbys
        SET player2_client = @player2Client
        WHERE invite_code = @inviteCode AND player2_client IS NULL";

            using var conn = _dbConnect.GetConnection();
            using var cmd = new NpgsqlCommand(sqlQuery, conn);
            cmd.Parameters.AddWithValue("inviteCode", inviteCode);
            cmd.Parameters.AddWithValue("player2Client", player2Client);

            int rowsAffected = cmd.ExecuteNonQuery();
            Console.WriteLine(rowsAffected);
            return rowsAffected > 0;
        }
    }
}
