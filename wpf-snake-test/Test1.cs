using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using wpf_snake.Models;
using wpf_snake.ViewModels;
using wpf_snake.Services;
using wpf_snake.Persistence;

namespace wpf_snake_test
{
    internal sealed class CurrentDirectoryScope : IDisposable
    {
        private readonly string _prev;
        public CurrentDirectoryScope(string newDir)
        {
            _prev = Directory.GetCurrentDirectory();
            Directory.SetCurrentDirectory(newDir);
        }
        public void Dispose() => Directory.SetCurrentDirectory(_prev);
    }

    internal sealed class FakeNavigationService : INavigationService
    {
        public bool ChooseOpened { get; private set; }
        public bool GameOpened { get; private set; }
        public bool GameOverOpened { get; private set; }
        public bool MainMenuOpened { get; private set; }

        public int? OpenedCellSize { get; private set; }
        public int? OpenedN { get; private set; }
        public int? GameOverScore { get; private set; }

        public void OpenChoose() => ChooseOpened = true;
        public void OpenGame(int cellSize, int n)
        {
            GameOpened = true;
            OpenedCellSize = cellSize;
            OpenedN = n;
        }
        public void OpenGameOver(int score)
        {
            GameOverOpened = true;
            GameOverScore = score;
        }
        public void OpenMainMenu() => MainMenuOpened = true;
    }

    [TestClass]
    public sealed class SnakeTests
    {
        [TestMethod]
        public void Move_NonGrowing_RemovesTail()
        {
            var snake = new Snake(new Point(5, 5));
            int initialLength = snake.body.Count;
            snake.direction = Direction.Right;
            snake.Move(grow: false);
            Assert.AreEqual(initialLength, snake.body.Count, "Length should remain same when not growing.");
        }

        [TestMethod]
        public void Move_Growing_IncreasesLength()
        {
            var snake = new Snake(new Point(5, 5));
            int initialLength = snake.body.Count;
            snake.direction = Direction.Right;
            snake.Move(grow: true);
            Assert.AreEqual(initialLength + 1, snake.body.Count, "Length should increase by one when growing.");
        }

        [TestMethod]
        public void IsCollisionWithSelf_WhenHeadOverlapsBody()
        {
            var snake = new Snake(new Point(2, 2));
            snake.body.Clear();
            snake.body.AddRange(new[]
            {
               new Point(2, 2),
                new Point(3, 2),
                new Point(4, 2),
                new Point(5, 2),
            });
            snake.direction = Direction.Right;
            snake.Move(grow: false);
            Assert.IsTrue(snake.IsCollisionWithSelf());

        }
    }

    [TestClass]
    public sealed class FoodTests
    {
        [TestMethod]
        public void Respawn_DoesNotPlaceOnSnake()
        {
            var snakeBody = new List<Point>
            {
                new Point(1,1),
                new Point(2,1),
                new Point(3,1)
            };
            var food = new Food(new Point(1, 1));
            food.Respawn(10, snakeBody);
            Assert.IsFalse(snakeBody.Contains(food.position));
        }
    }

    [TestClass]
    public sealed class GameStateTests
    {
        [TestMethod]
        public void Update_IncrementsScore()
        {
            var gs = new GameState(20);
            var foodProp = typeof(GameState).GetProperty("food");
            var head = gs.snake.body[0];
            foodProp!.SetValue(gs, new Food(head));
            int before = gs.score;
            gs.Update();
            Assert.AreEqual(before + 1, gs.score);
        }

        [TestMethod]
        public void Update_SetsGameOver_OnWallCollision()
        {
            var gs = new GameState(5);
            gs.snake.body.Clear();
            gs.snake.body.Add(new Point(2, 0));
            gs.snake.direction = Direction.Up;
            gs.Update();
            Assert.IsTrue(gs.isGameOver);
        }

        [TestMethod]
        public void Update_SetsGameOver_OnSelfCollision()
        {
            var gs = new GameState(10);
            gs.snake.body.Clear();
            gs.snake.body.AddRange(new[]
            {
                new Point(2, 2),
                new Point(3, 2),
                new Point(4, 2),
                new Point(5, 2),
            });
            gs.snake.direction = Direction.Right;
            gs.Update();
            Assert.IsTrue(gs.isGameOver);
        }
    }

    [DoNotParallelize]
    [TestClass]
    public sealed class FileManagementTests
    {
        private string _tempDir = "";

        [TestInitialize]
        public void Setup()
        {
            _tempDir = Path.Combine(Path.GetTempPath(), "wpf_snake_test_" + Guid.NewGuid());
            Directory.CreateDirectory(_tempDir);
            File.WriteAllLines(Path.Combine(_tempDir, "mapsize.txt"), new[]
            {
                "20;30",
                "15;25",
                "10;20"
            });
            File.WriteAllLines(Path.Combine(_tempDir, "scores.txt"), new[]
            {
                "Alice;5",
                "Bob;3"
            });
        }

        [TestCleanup]
        public void Cleanup()
        {
            if (Directory.Exists(_tempDir))
            {
                Directory.Delete(_tempDir, true);
            }
        }

        [TestMethod]
        public void LoadMapSize()
        {
            using var _ = new CurrentDirectoryScope(_tempDir);
            var fm = new FileManagement();
            MapSize ms = fm.loadMapSize(1);
            Assert.AreEqual(15, ms.cellSize);
            Assert.AreEqual(25, ms.n);
        }

        [TestMethod]
        public void SaveScore()
        {
            using var _ = new CurrentDirectoryScope(_tempDir);
            var fm = new FileManagement();
            fm.saveScore(10, "Carol");
            var lines = File.ReadAllLines(Path.Combine(_tempDir, "scores.txt"));
            Assert.IsTrue(lines.Contains("Carol;10"));
        }
    }

    [DoNotParallelize]
    [TestClass]
    public sealed class ChooseViewModelTests
    {
        [TestMethod]
        public void StartSmallMap()
        {
            var tempDir = Path.Combine(Path.GetTempPath(), "wpf_snake_test_" + Guid.NewGuid());
            Directory.CreateDirectory(tempDir);
            try
            {
                File.WriteAllLines(Path.Combine(tempDir, "mapsize.txt"), new[]
                {
                    "20;30",
                    "16;24",
                    "12;18"
                });

                using var _ = new CurrentDirectoryScope(tempDir);

                var fakeNav = new FakeNavigationService();
                var vm = new ChooseViewModel(fakeNav);
                vm.StartSmallMap();
                Assert.IsNotNull(vm.MapSize);
                Assert.AreEqual(20, vm.MapSize.cellSize);
                Assert.AreEqual(30, vm.MapSize.n);
                Assert.IsTrue(fakeNav.GameOpened);
                Assert.AreEqual(20, fakeNav.OpenedCellSize);
                Assert.AreEqual(30, fakeNav.OpenedN);
            }
            finally
            {
                Directory.Delete(tempDir, true);
            }
        }
    }

    [TestClass]
    public sealed class GameViewModelTests
    {
        private static MethodInfo GetPrivateMethod(object obj, string name) =>
            obj.GetType().GetMethod(name, BindingFlags.Instance | BindingFlags.NonPublic)!;

        [TestMethod]
        public void KeyPressChangesDirection()
        {
            var fakeNav = new FakeNavigationService();
            var vm = new GameViewModel(20, 15, fakeNav);
            vm.KeyPressCommand.Execute("Up");
            Assert.AreEqual(Direction.Up, vm.Model.snake.direction);
            vm.KeyPressCommand.Execute("Left");
            Assert.AreEqual(Direction.Left, vm.Model.snake.direction);
        }

        [TestMethod]
        public void KeyPressTogglesPauseTest()
        {
            var fakeNav = new FakeNavigationService();
            var vm = new GameViewModel(20, 15, fakeNav);
            vm.KeyPressCommand.Execute("P");
            Assert.IsTrue(vm.IsPaused);
            vm.KeyPressCommand.Execute("P");
            Assert.IsFalse(vm.IsPaused);
        }

        [TestMethod]
        public void NavigateGameOver()
        {
            var fakeNav = new FakeNavigationService();
            var vm = new GameViewModel(20, 5, fakeNav);

            vm.Model.snake.body.Clear();
            vm.Model.snake.body.Add(new Point(0, 2));
            vm.Model.snake.direction = Direction.Left;

            var update = GetPrivateMethod(vm, "UpdateGame");
            update.Invoke(vm, null);

            Assert.IsTrue(vm.Model.isGameOver);
            Assert.IsTrue(fakeNav.GameOverOpened);
        }
    }

    [TestClass]
    public sealed class GameOverViewModelTests
    {
        [DoNotParallelize]
        [TestMethod]
        public void SaveAndReturn()
        {
            var tempDir = Path.Combine(Path.GetTempPath(), "wpf_snake_test_" + Guid.NewGuid());
            Directory.CreateDirectory(tempDir);
            try
            {
                var scoresPath = Path.Combine(tempDir, "scores.txt");
                File.WriteAllLines(scoresPath, new[]
                {
                    "Alice;5",
                    "Bob;3"
                });

                using var _ = new CurrentDirectoryScope(tempDir);

                var fakeNav = new FakeNavigationService();
                var vm = new GameOverViewModel(8, fakeNav)
                {
                    PlayerName = "Dana"
                };

                var mi = typeof(GameOverViewModel).GetMethod("SaveAndReturn", BindingFlags.Instance | BindingFlags.NonPublic)!;
                mi.Invoke(vm, null);

                var lines = File.ReadAllLines(scoresPath);
                Assert.IsTrue(lines.Any(l => l.StartsWith("Dana;8")));
                Assert.IsTrue(fakeNav.MainMenuOpened);
            }
            finally
            {
                Directory.Delete(tempDir, true);
            }
        }
    }
}