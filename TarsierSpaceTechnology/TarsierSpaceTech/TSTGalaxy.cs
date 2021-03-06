﻿/*
 * TSTGalaxy.cs
 * (C) Copyright 2015, Jamie Leighton
 * Tarsier Space Technologies
 * The original code and concept of TarsierSpaceTech rights go to Tobyb121 on the Kerbal Space Program Forums, which was covered by the MIT license.
 * Original License is here: https://github.com/JPLRepo/TarsierSpaceTechnology/blob/master/LICENSE
 * As such this code continues to be covered by MIT license.
 * Kerbal Space Program is Copyright (C) 2013 Squad. See http://kerbalspaceprogram.com/. This
 * project is in no way associated with nor endorsed by Squad.
 *
 *  This file is part of TarsierSpaceTech.
 *
 *  TarsierSpaceTech is free software: you can redistribute it and/or modify
 *  it under the terms of the MIT License 
 *
 *  TarsierSpaceTech is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  
 *
 *  You should have received a copy of the MIT License
 *  along with TarsierSpaceTech.  If not, see <http://opensource.org/licenses/MIT>.
 *
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace TarsierSpaceTech
{
    public class TSTGalaxy : MonoBehaviour, ITargetable
    {

        private Orbit _galaxy_orbit = null;
        private OrbitDriver _galaxy_orbitdriver = null;
        private VesselTargetModes _galaxy_targetmodes = VesselTargetModes.Direction;

        private static Mesh mesh = null;

        private Material mat = new Material(Shader.Find("Unlit/Transparent"));
        public string theName;

        private ConfigNode config;

        private float _size = 1e3f;
        public float size
        {
            get { return _size * ScaledSpace.ScaleFactor; }
            set {
                _size = value / ScaledSpace.ScaleFactor;
                transform.localScale = _size * Vector3.one;
            }
        }
        
        public void Start()
        {
            if (mesh == null)
            {
                this.Log_Debug("Generating GalaxyMesh");
                mesh = new Mesh();
                mesh.vertices = new Vector3[]{
                    new Vector3(-1,0.75f,0),
                    new Vector3(-1,-0.75f,0),
                    new Vector3(1,0.75f,0),
                    new Vector3(1,-0.75f,0)
                };
                mesh.uv = new Vector2[]{
                    new Vector2(0, 1),
                    new Vector2(0, 0),
                    new Vector2(1, 1),
                    new Vector2(1, 0)
                };

                mesh.triangles = new int[]{
                    0,1,2,
                    3,2,1
                    };
               mesh.RecalculateNormals();
                
            }
            gameObject.GetComponent<MeshFilter>().mesh = mesh;
            renderer.material = mat;
            gameObject.layer = 10;
            renderer.castShadows = false;
            renderer.receiveShadows = false;
        }
        
        public void Update()
        {
            transform.LookAt(transform.parent.position);
        }

        public void Load(ConfigNode config)
        {
            this.config = config;
            string name = config.GetValue("name");
            string theName = config.GetValue("theName");
            Vector3 pos = ConfigNode.ParseVector3(config.GetValue("location"));
            string textureURL = config.GetValue("textureURL");
            float size = float.Parse(config.GetValue("size"));
            this.Log_Debug("Creating Galaxy: " + name + " " + pos.ToString() + " " + textureURL);
            this.Log_Debug("Setting Name");
            this.name = name;
            this.theName = theName;
            this.Log_Debug("Setting Size");
            this.size = 1e3f * size * ScaledSpace.ScaleFactor;
            this.Log_Debug("Setting Position");
            this.scaledPosition = -130e6f * pos.normalized;
            this.Log_Debug("Setting Texture");
            this.setTexture(GameDatabase.Instance.GetTexture(textureURL, false));
            this.Log_Debug("Finished creating galaxy");
        }

        public void attach(GameObject parent)
        {
            transform.parent = parent.transform;
        }
        
        public void setTexture(Texture texture)
        {
            mat.mainTexture = texture;
        }

        public Vector3 scaledPosition{
            get
            {
                return transform.localPosition;
            }
            set
            {
                transform.localPosition = value;
            }

        }

        public Vector3 position
        {
            get
            {
                return ScaledSpace.ScaledToLocalSpace(transform.position);
            }
            set{
                transform.position = ScaledSpace.LocalToScaledSpace(value);
            }
        }
        // ITargetable
        public Vector3 GetFwdVector()
        {
            return Vector3.zero;
        }
        
        public string GetName()
        {
            return this.name;
        }

        public Vector3 GetObtVelocity()
        {
            return Vector3.zero;
        }
        public Orbit GetOrbit()
        {
            return _galaxy_orbit;
        }
        
        public OrbitDriver GetOrbitDriver()
        {
            return _galaxy_orbitdriver;
        }
        
        public Vector3 GetSrfVelocity()
        {
            return Vector3.zero;
        }
        
        public VesselTargetModes GetTargetingMode()
        {
            return _galaxy_targetmodes;
        }
        
        public Transform GetTransform()
        {
            return this.transform;
        }
        
        public Vessel GetVessel()
        {
            return null;
        }

    }
}
