using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
 /// <summary>
/// classe de base pour contrôler un joueur
/// </summary>
public class Joueur : MonoBehaviour
{
    public float vitesse;
    
    [Header("Zoom Caméra si un événement est appelé")]
    //zoom de la camera si et ssi un bouton est appuyé
    public float zoomSize;

    [Header("position de la caméra devant le joueur si cette valeur > 0")]
    public float inFrontOf;

    [Header("Animator")]
    public Animator animator;
    public Animator animatorOmbre;

    protected Rigidbody2D rb2d;


    protected float time;
    protected CameraControllerJoy camerascript;

    ///<summary>
    ///position x et y du joystick sur sa position locale
    ///</summary>
    protected Vector2 v;

    protected float currentSpeed;

    protected bool lockPosition = false;

    public static Joueur currentPlayer;

    private void OnEnable()
    {
        currentPlayer = this;
    }

    protected void Start()
    {
        GameEvents.Pause += Pause;

        camerascript = CameraControllerJoy.Instance;
        rb2d = gameObject.GetComponent<Rigidbody2D>();

        camerascript.infrontOf = inFrontOf;
        //champ_de_vision_collider.radius = champ_de_vision;
    }


    protected void Update()
    {
        v = Joystick_Link.Instance.getPosition();

        if (lockPosition)
        {
            currentSpeed = 0.0f;
        }
        else
            // calcul de la vitesse du joueur en fonction du joystick
            currentSpeed = Mathf.Clamp(v.magnitude, 0.0f, 1.0f);

        Animate();
        Joystick_Link.Instance.setSpeed(currentSpeed * vitesse);
    }

    /// <summary>
    /// mise en pause du script
    /// </summary>
    protected void Pause()
    {
        enabled = ! enabled;
    }


    /// <summary>
    /// Animations du joueur
    /// </summary>
    void Animate()
    {
        if (v != Vector2.zero)
        {
            animator.SetFloat("Horizontal", v.y);
            animator.SetFloat("Vertical", v.x);
        }
        animator.SetFloat("Speed", currentSpeed);
        
        if (Global.Personnage == "Chamois")
        {
           //Debug.Log("L'ombre du chamois??? depuis Joueur");
            if (v != Vector2.zero)
            {
                animatorOmbre.SetFloat("Horizontal", v.y);
                animatorOmbre.SetFloat("Vertical", v.x);
            }
            animatorOmbre.SetFloat("Speed", currentSpeed);
        }
    }

    
    public void OnTriggerEnter2D(Collider2D col) {
       
        //TC : Pour le chasseur, il faut une balle pour tuer la cible...sinon entrer dans la zone valide la quête...
        // Refactoring : ces traitements pourraient être déplacés dans le script de gestion des triggers: TriggerZonePhoto...
        if (Global.Personnage=="Chamois" || Global.Personnage == "Randonneur" )
        {
           // TC Pour achever les quêtes de zone...
            if (col.CompareTag("Target"))
            {
                QuestManager.Instance.endQuest();
            }
        }
        
        /*
        if (Global.Personnage=="Chasseur") 
        {
            if (col.CompareTag("PhotoSite"))
            {
                GOPointer.PhotoBtn.SetActive(true);
                //col.tag="Zone";
                //QuestManager.Instance.endQuest();
                // Passage au site suivant...
            }
        }*/
        
    }

    public void OnTriggerExit2D(Collider2D col) {
    
        //GOPointer.PhotoBtn.SetActive(false);
    }
    
}
