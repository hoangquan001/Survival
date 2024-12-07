
using UnityEditor;
using UnityEngine;


public interface ICommand
{
    public void Execute(object o) { }

}

public interface ICommandExecutor
{
    public void Execute(ICommand command) { }
}