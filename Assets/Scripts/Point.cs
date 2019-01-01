using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace curve
{
    public class Point
    {
        public const float DistanceBetweenControlPoint=1.0f;
        public enum JoinType
        {
            Line,
            BezierSymmetrical,
            BezierIndependant
        }

        protected JoinType m_JoinType;
        public Vector3 position;
        public Vector3 controlOne;
        public Vector3 controlTwo;

        
        public Point(JoinType joinType,Vector3 position)
        {
            m_JoinType = joinType;
            this.position = position;
        }

        public Point(JoinType joinType, Vector3 position,Vector3 controlOne,Vector3 controlTwo)
        {
            m_JoinType = joinType;
            this.position = position;
            this.controlOne = controlOne;
            this.controlTwo = controlTwo;
        }

        public JoinType joinType
        {
            get
            {
                return m_JoinType;
            }
            set
            {
                m_JoinType = value;
            }
        }


    }
}