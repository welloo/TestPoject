using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Match3Test.HandlerScripts
{
    public abstract class GameObjectModel : IGameObjectModel
    {

        public event Action<string> OnDelete;
        public event Action<string, string> ChangedName;
        public Rectangle Rectangle;
        public string Name
        {
            get => name;
            set
            {
                if (parentModel == null) { name = value; }
                else if (!(parentModel.HasModel(value)))
                {
                    ChangedName?.Invoke(name, value);
                    name = value;
                }

            }
        }

        protected GameObjectModel parentModel;
        protected Dictionary<string, GameObjectModel> childModels;
        protected string name;
        protected GameObjectModel(string name)
        {
            Name = name;
            childModels = new Dictionary<string, GameObjectModel>();
        }

        public List<GameObjectModel> GetChildList() => childModels.Values.ToList();
        public GameObjectModel GetParent() => parentModel;
        public virtual void LoadContent() => GetChildList().ForEach(model => model.LoadContent());
        public virtual void Initialize() => GetChildList().ForEach(model => model.Initialize());
        public virtual void Update(GameTime gameTime) => GetChildList().ForEach(model => model.Update(gameTime));
        public virtual void Draw(GameTime gameTime) => GetChildList().ForEach(model => model.Draw(gameTime));
        public virtual void Dispose()=> UnloadContent();
        public virtual void UnloadContent()
        {
            OnDelete?.Invoke(Name);
            GetChildList().ForEach(model => model.UnloadContent());
        }


        public void AddChild(GameObjectModel newChild)
        {
            string newName = newChild.Name;
            if (HasModel(newName))
            {
                ushort index = 1;

                while (HasModel(newName))
                {
                    newName = newChild.Name + "_" + index;
                    index++;
                }
                newChild.Name = newName;
            }
            childModels.Add(newChild.Name, newChild);
            newChild.parentModel = this;
            newChild.ChangedName = OnChildChangedName;
            newChild.OnDelete = RemoveChild;
            newChild.Initialize();
            newChild.LoadContent();
        }



        private void OnChildChangedName(string oldName, string newName)
        {
            GameObjectModel model = GetModel(oldName);
            childModels.Remove(oldName);
            childModels.Add(newName, model);
        }

        public GameObjectModel GetModel(string name)
        {
            childModels.TryGetValue(name, out GameObjectModel result);
            if (result == null)
                throw new Exception($"Model {name} not found");
            return result;
        }

        public T GetModel<T>(string name)
            where T : GameObjectModel
        {
            childModels.TryGetValue(name, out GameObjectModel result);
            if (result == null) 
                throw new Exception($"Model {name} not found");
            T model = result as T;
            if (model == null) 
                throw new Exception($"Not correctly type of {name}");
            return model;
        }

        public bool HasModel(string name)
        {
            childModels.TryGetValue(name, out GameObjectModel result);
            return (result != null);
        }

        public void RemoveChild(string childName)
        {
            GameObjectModel model = GetModel(childName);
            model.ChangedName = null;
            childModels.Remove(childName);
        }

        public void SetPosition(int x, int y)
        {
            if (parentModel != null)
            {
                x += parentModel.Rectangle.X;
                y += parentModel.Rectangle.Y;
            }
            Rectangle.Location = new Point(x, y);
        }

        public void SetPosition(Point point)
        {
            if (parentModel != null)
            {
                point.X += parentModel.Rectangle.X;
                point.Y += parentModel.Rectangle.Y;
            }
            Rectangle.Location = point;
        }

        public Point GetPosition() => parentModel != null ? Rectangle.Location - parentModel.Rectangle.Location : Rectangle.Location;
        public Point GetGlobalPosition() => Rectangle.Location;
        public void SetGlobalPosition(int x, int y) => Rectangle.Location = new Point(x, y);
        public void SetSize(int x, int y) => Rectangle.Size = new Point(x, y);
    }
}

