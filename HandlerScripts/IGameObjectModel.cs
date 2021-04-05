using Microsoft.Xna.Framework;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Match3Test.HandlerScripts
{
    public interface IGameObjectModel : IDisposable
    {
        string Name { get; set; }

        event Action<string, string> ChangedName;
        event Action<string> OnDelete;

        void AddChild(GameObjectModel newChild);
        void Draw(GameTime gameTime);
        List<GameObjectModel> GetChildList();
        Point GetGlobalPosition();
        GameObjectModel GetModel(string name);
        T GetModel<T>(string name) where T : GameObjectModel;
        GameObjectModel GetParent();
        Point GetPosition();
        bool HasModel(string name);
        void Initialize();
        void LoadContent();
        void RemoveChild(string childName);
        void SetGlobalPosition(int x, int y);
        void SetPosition(int x, int y);
        void SetPosition(Point point);
        void SetSize(int x, int y);
        void UnloadContent();
        void Update(GameTime gameTime);
    }
}