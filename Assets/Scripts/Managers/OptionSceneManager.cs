using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OptionSceneManager : MonoBehaviour {
    // Agent Status
    public TMP_InputField Agent_HealthPoint;
    public TMP_InputField Agent_AttackPoint;
    public TMP_InputField Agent_FireRate;
    public TMP_InputField Agent_MagazineSize;
    public TMP_InputField Agent_MoveSpeed;
    public TMP_InputField Agent_RotateSpeed;
    public TMP_InputField Agent_ViewDistance;
    // Spawn Rate
    public TMP_InputField SpawnRate_Zombie;
    public TMP_InputField SpawnRate_Spitter;
    public TMP_InputField SpawnRate_Tank;
    public TMP_InputField SpawnRate_Charger;
    public TMP_InputField SpawnRate_Wanderer;
    // Zombie Status
    public TMP_InputField Zombie_HealthPoint;
    public TMP_InputField Zombie_AttackPoint;
    public TMP_InputField Zombie_MoveSpeed;
    public TMP_InputField Zombie_RotateSpeed;
    // Wander Status
    public TMP_InputField Wanderer_HealthPoint;
    public TMP_InputField Wanderer_AttackPoint;
    public TMP_InputField Wanderer_MoveSpeed;
    public TMP_InputField Wanderer_RotateSpeed;
    public TMP_InputField Wanderer_ViewDistance;
    // Tank Status
    public TMP_InputField Tank_HealthPoint;
    public TMP_InputField Tank_AttackPoint;
    public TMP_InputField Tank_MoveSpeed;
    public TMP_InputField Tank_RotateSpeed;
    public TMP_InputField Tank_ViewDistance;
    // Spitter Status
    public TMP_InputField Spitter_HealthPoint;
    public TMP_InputField Spitter_AttackPoint;
    public TMP_InputField Spitter_MoveSpeed;
    public TMP_InputField Spitter_RotateSpeed;
    public TMP_InputField Spitter_FireRate;
    public TMP_InputField Spitter_ViewDistance;
    // Charger Status
    public TMP_InputField Charger_HealthPoint;
    public TMP_InputField Charger_AttackPoint;
    public TMP_InputField Charger_MoveSpeed;
    public TMP_InputField Charger_RotateSpeed;
    public TMP_InputField Charger_ChargeSpeed;
    public TMP_InputField Charger_FireRate;
    public TMP_InputField Charger_ViewDistance;

    private void Awake() {
        LoadSettings();
    }

    public void ChangeToMenuScene() {
        SceneManager.LoadScene("Menu");
    }

    public void ApplySettings() {

        GameSettingManager.options.SpawnRate_Zombie = int.Parse(SpawnRate_Zombie.text);
        GameSettingManager.options.SpawnRate_Spitter = int.Parse(SpawnRate_Spitter.text);
        GameSettingManager.options.SpawnRate_Tank = int.Parse(SpawnRate_Tank.text);
        GameSettingManager.options.SpawnRate_Wanderer = int.Parse(SpawnRate_Wanderer.text);
        GameSettingManager.options.SpawnRate_Charger = int.Parse(SpawnRate_Charger.text);

        GameSettingManager.options.Agent_HealthPoint = int.Parse(Agent_HealthPoint.text);
        GameSettingManager.options.Agent_AttackPoint = int.Parse(Agent_AttackPoint.text);
        GameSettingManager.options.Agent_FireRate = float.Parse(Agent_FireRate.text);
        GameSettingManager.options.Agent_MagazineSize = int.Parse(Agent_MagazineSize.text);
        GameSettingManager.options.Agent_MoveSpeed = float.Parse(Agent_MoveSpeed.text);
        GameSettingManager.options.Agent_RotateSpeed = float.Parse(Agent_RotateSpeed.text);
        GameSettingManager.options.Agent_ViewDistance = float.Parse(Agent_ViewDistance.text);


        GameSettingManager.options.Zombie_HealthPoint = int.Parse(Zombie_HealthPoint.text);
        GameSettingManager.options.Zombie_AttackPoint = int.Parse(Zombie_AttackPoint.text);
        GameSettingManager.options.Zombie_MoveSpeed = float.Parse(Zombie_MoveSpeed.text);
        GameSettingManager.options.Zombie_RotateSpeed = float.Parse(Zombie_RotateSpeed.text);

        GameSettingManager.options.Spitter_HealthPoint = int.Parse(Spitter_HealthPoint.text);
        GameSettingManager.options.Spitter_AttackPoint = int.Parse(Spitter_AttackPoint.text);
        GameSettingManager.options.Spitter_MoveSpeed = float.Parse(Spitter_MoveSpeed.text);
        GameSettingManager.options.Spitter_RotateSpeed = float.Parse(Spitter_RotateSpeed.text);
        GameSettingManager.options.Spitter_FireRate = float.Parse(Spitter_FireRate.text);
        GameSettingManager.options.Spitter_ViewDistance = float.Parse(Spitter_ViewDistance.text);

        GameSettingManager.options.Tank_HealthPoint = int.Parse(Tank_HealthPoint.text);
        GameSettingManager.options.Tank_AttackPoint = int.Parse(Tank_AttackPoint.text);
        GameSettingManager.options.Tank_MoveSpeed = float.Parse(Tank_MoveSpeed.text);
        GameSettingManager.options.Tank_RotateSpeed = float.Parse(Tank_RotateSpeed.text);
        GameSettingManager.options.Tank_ViewDistance = float.Parse(Tank_ViewDistance.text);

        GameSettingManager.options.Charger_HealthPoint = int.Parse(Charger_HealthPoint.text);
        GameSettingManager.options.Charger_AttackPoint = int.Parse(Charger_AttackPoint.text);
        GameSettingManager.options.Charger_MoveSpeed = float.Parse(Charger_MoveSpeed.text);
        GameSettingManager.options.Charger_RotateSpeed = float.Parse(Charger_RotateSpeed.text);
        GameSettingManager.options.Charger_ChargeSpeed = float.Parse(Charger_ChargeSpeed.text);
        GameSettingManager.options.Charger_FireRate = float.Parse(Charger_FireRate.text);
        GameSettingManager.options.Charger_ViewDistance = float.Parse(Charger_ViewDistance.text);

        GameSettingManager.options.Wanderer_HealthPoint = int.Parse(Wanderer_HealthPoint.text);
        GameSettingManager.options.Wanderer_AttackPoint = int.Parse(Wanderer_AttackPoint.text);
        GameSettingManager.options.Wanderer_MoveSpeed = float.Parse(Wanderer_MoveSpeed.text);
        GameSettingManager.options.Wanderer_RotateSpeed = float.Parse(Wanderer_RotateSpeed.text);
        GameSettingManager.options.Wanderer_ViewDistance = float.Parse(Wanderer_ViewDistance.text);
        GameSettingManager.SaveSettings();
    }
    public void LoadSettings() {
        GameSettingManager.LoadSettings();
        Agent_HealthPoint.text = GameSettingManager.options.Agent_HealthPoint.ToString();
        Agent_AttackPoint.text = GameSettingManager.options.Agent_AttackPoint.ToString();
        Agent_FireRate.text = GameSettingManager.options.Agent_FireRate.ToString();
        Agent_MagazineSize.text = GameSettingManager.options.Agent_MagazineSize.ToString();
        Agent_MoveSpeed.text = GameSettingManager.options.Agent_MoveSpeed.ToString();
        Agent_RotateSpeed.text = GameSettingManager.options.Agent_RotateSpeed.ToString();
        Agent_ViewDistance.text = GameSettingManager.options.Agent_ViewDistance.ToString();

        SpawnRate_Zombie.text = GameSettingManager.options.SpawnRate_Zombie.ToString();
        SpawnRate_Tank.text = GameSettingManager.options.SpawnRate_Tank.ToString();
        SpawnRate_Wanderer.text = GameSettingManager.options.SpawnRate_Wanderer.ToString();
        SpawnRate_Charger.text = GameSettingManager.options.SpawnRate_Charger.ToString();

        Zombie_HealthPoint.text = GameSettingManager.options.Zombie_HealthPoint.ToString();
        Zombie_AttackPoint.text = GameSettingManager.options.Zombie_AttackPoint.ToString();
        Zombie_MoveSpeed.text = GameSettingManager.options.Zombie_MoveSpeed.ToString();
        Zombie_RotateSpeed.text = GameSettingManager.options.Zombie_RotateSpeed.ToString();

        Spitter_HealthPoint.text = GameSettingManager.options.Spitter_HealthPoint.ToString();
        Spitter_AttackPoint.text = GameSettingManager.options.Spitter_AttackPoint.ToString();
        Spitter_MoveSpeed.text = GameSettingManager.options.Spitter_MoveSpeed.ToString();
        Spitter_RotateSpeed.text = GameSettingManager.options.Spitter_RotateSpeed.ToString();
        Spitter_FireRate.text = GameSettingManager.options.Spitter_FireRate.ToString();
        Spitter_ViewDistance.text = GameSettingManager.options.Spitter_ViewDistance.ToString();

        Tank_HealthPoint.text = GameSettingManager.options.Tank_HealthPoint.ToString();
        Tank_AttackPoint.text = GameSettingManager.options.Tank_AttackPoint.ToString();
        Tank_MoveSpeed.text = GameSettingManager.options.Tank_MoveSpeed.ToString();
        Tank_RotateSpeed.text = GameSettingManager.options.Tank_RotateSpeed.ToString();
        Tank_ViewDistance.text = GameSettingManager.options.Tank_ViewDistance.ToString();

        Charger_HealthPoint.text = GameSettingManager.options.Charger_HealthPoint.ToString();
        Charger_AttackPoint.text = GameSettingManager.options.Charger_AttackPoint.ToString();
        Charger_MoveSpeed.text = GameSettingManager.options.Charger_MoveSpeed.ToString();
        Charger_RotateSpeed.text = GameSettingManager.options.Charger_RotateSpeed.ToString();
        Charger_ChargeSpeed.text = GameSettingManager.options.Charger_ChargeSpeed.ToString();
        Charger_FireRate.text = GameSettingManager.options.Charger_FireRate.ToString();
        Charger_ViewDistance.text = GameSettingManager.options.Charger_ViewDistance.ToString();

        Wanderer_HealthPoint.text = GameSettingManager.options.Wanderer_HealthPoint.ToString();
        Wanderer_AttackPoint.text = GameSettingManager.options.Wanderer_AttackPoint.ToString();
        Wanderer_MoveSpeed.text = GameSettingManager.options.Wanderer_MoveSpeed.ToString();
        Wanderer_RotateSpeed.text = GameSettingManager.options.Wanderer_RotateSpeed.ToString();
        Wanderer_ViewDistance.text = GameSettingManager.options.Wanderer_ViewDistance.ToString();

    }


}

