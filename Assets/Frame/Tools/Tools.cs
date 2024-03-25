using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Frame.Tools
{
    public static class Tools
    {

        /// <summary>
        /// 绕指定轴旋转给定点。
        /// </summary>
        /// <param name="oldPosition">旋转前的点。</param>
        /// <param name="rotateCenter">旋转中心点。</param>
        /// <param name="axis">旋转轴。</param>
        /// <param name="angle">旋转角度（度数）。</param>
        /// <returns>旋转后的点。</returns>
        public static Vector3 RotateAround(Vector3 oldPosition, Vector3 rotateCenter, Vector3 axis, float angle)
        {
            Vector3 direction = oldPosition - rotateCenter;
            Quaternion rotation = Quaternion.AngleAxis(angle, axis);
            Vector3 rotatedDirection = rotation * direction;
            return rotateCenter + rotatedDirection;
        }

        /// <summary>
        /// 根据速度和总时间计算指定轴上的位置。
        /// </summary>
        /// <param name="BeginPoint">起始点。</param>
        /// <param name="RotationCenter">旋转中心。</param>
        /// <param name="Axis">旋转轴。</param>
        /// <param name="Speed">旋转速度。</param>
        /// <param name="totalTime">总时间。</param>
        /// <returns>计算得到的位置。</returns>
        public static Vector3 GetPosition(Vector3 BeginPoint, Vector3 RotationCenter, Vector3 Axis, float Speed, float totalTime)
        {
            // 根据速度和总时间计算旋转角度
            float angle = Speed * totalTime;

            // 将角度转换为弧度
            angle *= Mathf.Deg2Rad;

            // 获取旋转矩阵
            Quaternion rotation = Quaternion.AngleAxis(angle, Axis);

            // 将旋转中心作为原点
            Vector3 relativePosition = BeginPoint - RotationCenter;

            // 应用旋转
            Vector3 rotatedPosition = rotation * relativePosition;

            // 将旋转后的位置加上旋转中心的偏移量
            Vector3 finalPosition = RotationCenter + rotatedPosition;

            return finalPosition;
        }

        /// <summary>
        /// 逐步将给定 Transform 旋转至指定的前方和上方。
        /// </summary>
        /// <param name="transform">要旋转的 Transform。</param>
        /// <param name="targetForward">目标前方向。</param>
        /// <param name="targetUp">目标上方向。</param>
        /// <param name="maxAngle">每秒最大旋转角度。</param>
        public static void StepRotateTowards(Transform transform, Vector3 targetForward, Vector3 targetUp, float maxAngle)
        {
            // 计算当前朝向
            Vector3 currentForward = transform.forward;
            Vector3 currentUp = transform.up;

            // 计算目标旋转
            Quaternion targetRotation = Quaternion.LookRotation(targetForward, targetUp);

            // 限制每秒旋转的角度
            float maxRotationAngle = maxAngle * Time.deltaTime;

            // 通过 Quaternion.RotateTowards 逐步旋转
            Quaternion newRotation = Quaternion.RotateTowards(transform.rotation, targetRotation, maxRotationAngle);

            // 应用新的旋转
            transform.rotation = newRotation;
        }
    }
}
