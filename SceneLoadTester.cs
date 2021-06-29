using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Firebase;
using Firebase.Database;
using Firebase.Unity;


public class SceneLoadTester : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://android-0000-firebase.firebaseio.com/");
    }

    Firebase.Auth.FirebaseAuth auth;
    DatabaseReference reference;


    void Awake()
    {
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        reference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    Firebase.Auth.FirebaseUser user_info = null;


    [SerializeField] InputField if_email;
    [SerializeField] InputField if_password;
    [SerializeField] Button btn_SignUp;
    [SerializeField] Button btn_LogIn;

    public void SignUp()
    {
        if (if_email.text.Length != 0 && if_password.text.Length != 0)
        {
            auth.CreateUserWithEmailAndPasswordAsync(if_email.text, if_password.text).ContinueWith(
                task =>
                {
                    if (!task.IsCanceled && !task.IsFaulted)
                    {
                        user_info = task.Result;

                        Debug.Log("회원가입 성공");

                        Dictionary<string, object> childupdate = new Dictionary<string, object>();
                        //childupdate["/1_이름/"] = username.text;
                        childupdate["/2_이메일/"] = if_email.text;
                        childupdate["/3_비밀번호/"] = if_password.text;
                        //childupdate["/4_구분/"] = type.text;
                        //childupdate["/5_소속대학/"] = university.text;
                        //childupdate["/6_소속학과/"] = major.text;


                        reference.Child("user_info").Child(user_info.UserId).UpdateChildrenAsync(childupdate);

                        Debug.Log("회원정보 저장");
                    }
                    else
                    {
                        Debug.Log("회원가입 실패");
                    }
                });
        }
    }

    public bool bool_login = false;

    public void SignIn()
    {
        if (if_email.text.Length != 0 && if_password.text.Length != 0)
        {
            auth.SignInWithEmailAndPasswordAsync(if_email.text, if_password.text).ContinueWith(
                task =>
                {
                    if (task.IsCompleted && !task.IsCanceled && !task.IsFaulted)
                    {
                        user_info = task.Result;
                        Debug.Log("로그인 성공");
                        bool_login = true;
                        //SceneChange();
                        //Read_userinfo();
                        //SceneManager.LoadScene("Scene2");
                        
                    }
                    else
                    {
                        Debug.Log("로그인 실패");
                        //popup_bool = true;
                    }
                });
        }
        else
        { }
    }


    void Update()
    {
        if (bool_login == true)
        {
            SceneManager.LoadScene("Scene2");
            Debug.Log("ㅠ");
        }
        else { }
    }


    public void SaveData()
    {
        Dictionary<string, object> childupdate = new Dictionary<string, object>();
        childupdate["/1_이름/"] = "김교육".ToString();
        reference.Child("user_info").Child(user_info.UserId).UpdateChildrenAsync(childupdate);
    }

    /*/
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            LoadingSceneController.LoadScene("Scene2");
        }
    }
    /*/


}
