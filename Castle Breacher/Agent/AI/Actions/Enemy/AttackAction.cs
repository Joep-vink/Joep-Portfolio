
public class AttackAction : AIAction
{
    public override void TakeAction()
    {
        aIActionData.Attack = true;
        aiBrain.Attack();
    }
}
