<template>
  <el-form label-width="auto" label-suffix=":" style="min-width: 400px">
    <el-form-item label="用户邮箱">
      {{ referrer.user.email }}
    </el-form-item>
    <el-form-item label="推荐等级">
      <LevelTag :value="referrer.level" v-if="referrer.level" />
      <span v-else>未设置</span>
    </el-form-item>
    <el-form-item label="直推人数">
      {{ referrer.directCount }}
    </el-form-item>
    <el-form-item label="间推人数">
      {{ referrer.indirectCount }}
    </el-form-item>
    <el-form-item label="总推荐人数">
      {{ referrer.totalCount }}
    </el-form-item>
    <el-form-item label="可用佣金">
      {{ referrer.commission }} USD
    </el-form-item>
    <el-form-item label="累计佣金">
      {{ referrer.totalCommission }} USD
    </el-form-item>
    <el-form-item label="已提现金额">
      {{ referrer.withdrawal }} USD
    </el-form-item>
    <el-form-item label="提现地址">
      {{ referrer.withdrawalAddress || '未设置' }}
    </el-form-item>
    <el-form-item label="状态">
      <el-tag :type="referrer.isDisabled ? 'danger' : 'success'">
        {{ referrer.isDisabled ? '禁用' : '正常' }}
      </el-tag>
    </el-form-item>
    <el-form-item label="备注">
      {{ referrer.remark || '无' }}
    </el-form-item>
    <el-form-item label="创建时间">
      <Datetime :value="referrer.createdAt" />
    </el-form-item>
    <el-form-item label="更新时间" v-if="referrer.updatedAt">
      <Datetime :value="referrer.updatedAt" />
    </el-form-item>
  </el-form>
</template>

<script setup lang="ts">
  import { ReferrerWithDetailsDto } from '@/api/services'
  import LevelTag from '../../levelTag.vue'

  const { referrer } = defineProps<{
    referrer: ReferrerWithDetailsDto
  }>()
</script>

<style scoped lang="scss"></style>