using UnityEditor;

namespace Sierra.AGPW.TenSecondAscencion
{
    [CustomEditor(typeof(PlayerController))]
    public class PlayerControllerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            var pc = target as PlayerController;

            base.OnInspectorGUI();
        }
    }
}