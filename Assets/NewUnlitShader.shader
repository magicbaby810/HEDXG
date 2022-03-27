Shader "Custom/NewShader" {
	Properties 
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Cutoff("Alpha Cutoff", Range(0, 1)) = 0.1
		_Color("Color",Color) = (1,1,1,1)
	}
	SubShader  
    {  
        Pass  
        {
			Material   
            {  
                Diffuse [_Color]  
                Ambient[_Color]  
            }
			Lighting On
            AlphaTest GEqual[_Cutoff]
			
			SetTexture[_MainTex] {}
			SetTexture[_MainTex] {combine texture + primary DOUBLE, previous} 
        }  
    }  
	FallBack "Diffuse"
}