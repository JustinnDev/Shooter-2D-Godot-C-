using Godot;
using System;

namespace Game
{
    [Serializable]
    public class Timer
    {
        public TimeScale timeScale;
        public UpdateMode updateMode;
        public TypeOperation typeOperation;

        public float count { get; private set; }
        public float countLimit { get; private set; }
        public float defaultLimit { get; private set; }

        //Hay que quitar las variables para visualizar desde el inspector ya que no las voy a usar

        [Export] private float _countLimit; //Para colocar desde el editor
        [Export] private float _count; //Variable para visualizar desde el editor
        [Export] private float _defaultLimit;

        private bool readyToUpdate;//Para que la accion de stopp solo se ejecute una vez 

        public void Update(bool whenToOperation, Action actionOfOperation, bool whenToStop, Action actionOfStop, double delta, bool actionReset = true)
        {
            SetValueOnEditor();

            if (whenToOperation)
            {
                if (actionOfOperation != null)
                    actionOfOperation();

                Add(delta);
            }

            if (whenToStop && readyToUpdate)
            {
                if (actionOfStop != null)
                    actionOfStop();

                if (actionReset)
                    Reset();
            }
        }

        //Constructores
        #region
        public void SetDefaultLimit(float limit = 0) => defaultLimit = limit == 0 ? _countLimit : limit;
        #endregion

        //Metodos Contador
        #region
        private void Add(double delta)
        {
            switch (timeScale)
            {
                case TimeScale.Scaled:
                    count = typeOperation == TypeOperation.Increment ?
                           count + (float)delta :
                           count - (float)delta;
                    break;
                case TimeScale.Unscaled:
                    count = typeOperation == TypeOperation.Increment ?
                           count + (float)delta :
                           count - (float)delta;
                    break;
            }

            readyToUpdate = true;
        }
        public void Reset()
        {
            count = 0;
            readyToUpdate = updateMode == UpdateMode.OnlyOnce ? false : true;
        }
        #endregion

        //Metodos Contador
        #region
        public void SetLimit(float limit) => _countLimit = Mathf.Abs(limit);
        public float GetLimit() => Mathf.Abs(_countLimit);
        public bool OverLimit() => Mathf.Abs(count) > Mathf.Abs(countLimit);
        public bool OnLimit() => Mathf.Abs(count) >= Mathf.Abs(countLimit);
        #endregion

        //Editor
        #region
        public void SetValueOnEditor()
        {
            _count = count;
            countLimit = _countLimit;
            _defaultLimit = defaultLimit;
        }
        #endregion

        public enum TimeScale
        {
            Scaled,
            Unscaled
        }

        public enum UpdateMode
        {
            Continuous,
            OnlyOnce
        }

        public enum TypeOperation
        {
            Increment,
            Decrement
        }
    }
}
