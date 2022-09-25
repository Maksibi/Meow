using System;
using SFML.Learning;
using SFML.Window;
using SFML.Graphics;

internal class Program : Game
{
    static string backgroundTexture = LoadTexture("background.png");
    static string playerTexture = LoadTexture("player.png");
    static string mouseTexture = LoadTexture("mouse.png");

    static string clickedSound = LoadSound("meow_sound.wav");
    static string crashSound = LoadSound("cat_crash_sound.wav");
    static string music = LoadMusic("bg_music.wav");

    static float playerX = 30;
    static float playerY = 220;
    static int playerSize = 56;
    static float playerSpeed = 400;
    static int playerDirection = 1;
    static float foodX;
    static float foodY;
    static int foodSize = 64;

    static int totalScore = 0;
    static int playerScore = 0;
    static void PlayerMove()
    {
        if (GetKey(SFML.Window.Keyboard.Key.W) == true) playerDirection = 0;
        if (GetKey(SFML.Window.Keyboard.Key.D) == true) playerDirection = 1;
        if (GetKey(SFML.Window.Keyboard.Key.S) == true) playerDirection = 2;
        if (GetKey(SFML.Window.Keyboard.Key.A) == true) playerDirection = 3;

        if (playerDirection == 0) playerY -= playerSpeed * DeltaTime;
        if (playerDirection == 1) playerX += playerSpeed * DeltaTime;
        if (playerDirection == 2) playerY += playerSpeed * DeltaTime;
        if (playerDirection == 3) playerX -= playerSpeed * DeltaTime;

        if (GetKeyUp(Keyboard.Key.K)) playerScore += 30;
    }
    static void DrawPlayer()
    {
        if (playerDirection == 0) DrawSprite(playerTexture, playerX, playerY, 64, 64, playerSize, playerSize);
        if (playerDirection == 1) DrawSprite(playerTexture, playerX, playerY, 0, 0, playerSize, playerSize);
        if (playerDirection == 2) DrawSprite(playerTexture, playerX, playerY, 0, 64, playerSize, playerSize);
        if (playerDirection == 3) DrawSprite(playerTexture, playerX, playerY, 64, 0, playerSize, playerSize);
    }

    static void Main(string[] args)
    {
        InitWindow(800, 600, "Meow");
        SetFont("comic.ttf");
        PlayMusic(music, 7);
        Random rnd = new Random();
        foodX = rnd.Next(0, 800 - foodSize);
        foodY = rnd.Next(0, 600 - foodSize);
        bool isLose = false;
        while (true)
        {
            DispatchEvents();
            if (isLose == false)
            {
                PlayerMove();
                if (playerX + playerSize > foodX && playerX < foodX + foodSize &&
                          playerY < foodY + foodSize && playerY + playerSize > foodY)
                {
                    PlaySound(clickedSound, 15);
                    foodX = rnd.Next(0, 800 - foodSize);
                    foodY = rnd.Next(0, 600 - foodSize);
                    playerScore++;
                    if (totalScore < playerScore)
                    {
                        totalScore = playerScore;
                    }
                    playerSpeed += 10;
                }
                if (playerX + playerSize > 800 || playerX < 0 || playerY + playerSize > 600 || playerY < 0)
                {
                    isLose = true;
                    PlaySound(crashSound);
                }
            }
                if (isLose == true)
                {
                    if (GetKey(Keyboard.Key.R))
                    {
                        playerX = 300;
                        playerY = 220;
                        playerScore = 0;
                        playerSpeed = 400;
                        playerDirection = 1;
                        isLose = false;
                    }
            }
                
            ClearWindow();
                DrawSprite(backgroundTexture, 0, 0);
                DrawPlayer();
            if (isLose == true)
            {
                SetFillColor(50, 50, 50);
                DrawText(200, 300, "Вы проиграли!", 24);
                SetFillColor(50, 50, 50);
                DrawText(232, 350, "Нажмите \"R\" для перезапуска.", 18);
            }
            SetFillColor(70, 70, 70);
                DrawText(20, 8, "Съедено: " + playerScore.ToString(), 18);
                DrawText(20, 24, "Рекорд: " + totalScore.ToString(), 18);
                DrawSprite(mouseTexture, foodX, foodY, 0, 0, foodSize, foodSize);
                DisplayWindow();

                Delay(1);
        }
    }
}
