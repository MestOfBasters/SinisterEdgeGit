using System;
using UnityEngine;
using UnityEngine.UI;

namespace UnityStandardAssets.Utility
{
   // [RequireComponent(typeof (GUIText))]
    public class FPSCounter : MonoBehaviour
    {
        public bool enableDebug;
        const float fpsMeasurePeriod = 1f;
        private int m_FpsAccumulator = 0;
        private float m_FpsNextPeriod = 0;
        private int m_CurrentFps;
        const string display = "{0} FPS";
        public Text m_GuiText;
        public Text terrainDetailsText;
        private int[] lastFrames;
        private int lastframesCounter = 0;
        public AnimationCurve grassViewDistance;
        public bool terrainDetailAutomation=false;
        void Awake()
        {
            
        }

        private void Start()
        {
            Application.targetFrameRate = 30;
			
            m_FpsNextPeriod = Time.realtimeSinceStartup + fpsMeasurePeriod;
            
            //initialize low settings:
            //Terrain.activeTerrain.detailObjectDistance = 5f;
            //Terrain.activeTerrain.detailObjectDensity = .2f;
        }


        private void Update()
        {
            
            // measure average frames per second
            m_FpsAccumulator++;
            if (Time.realtimeSinceStartup > m_FpsNextPeriod)
            {
                
                m_CurrentFps = (int) (m_FpsAccumulator/fpsMeasurePeriod);
                m_FpsAccumulator = 0;
                m_FpsNextPeriod += fpsMeasurePeriod;


                    m_GuiText.text = string.Format(display, m_CurrentFps);

                if (terrainDetailAutomation)
                {
                    if (m_CurrentFps >= 30f)
                    {
                       if (Terrain.activeTerrain.detailObjectDistance < 20f)
                                Terrain.activeTerrain.detailObjectDistance += 2f;

                     //  if(Terrain.activeTerrain.detailObjectDistance == 20f)
                     //           Terrain.activeTerrain.detailObjectDensity += .1f;
                        if(Terrain.activeTerrain.detailObjectDensity < 0.5f)
                            Terrain.activeTerrain.detailObjectDensity += .1f;
                    }
                    if (m_CurrentFps < 20f)
                    {
                        
                       // Terrain.activeTerrain.detailObjectDensity -= .2f;
                        if (Terrain.activeTerrain.detailObjectDistance > 10f)
                            Terrain.activeTerrain.detailObjectDistance -= 2f;
                        else if (Terrain.activeTerrain.detailObjectDistance == 10f &&
                                 Terrain.activeTerrain.detailObjectDensity > 0f)
                            Terrain.activeTerrain.detailObjectDensity -= .1f;
                    }
                }
                
            }

           
        }
    }
}
