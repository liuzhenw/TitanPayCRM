<template>
  <div class="warp">
    <el-row :gutter="16">
      <el-col :md="12" :sm="24">
        <el-card shadow="never">
          <template #header>
            <span>用户信息</span>
          </template>
          <el-form label-width="auto" label-suffix=":" style="min-width: 400px">
            <el-form-item label="用户名称">
              <EllipticalLabel :value="value.name" />
            </el-form-item>
            <el-form-item label="邮件地址">
              <EllipticalLabel :value="value.email" />
            </el-form-item>
            <el-form-item label="锁定时间" v-if="value.lockedAt">
              <Datetime :value="value.lockedAt" />
            </el-form-item>
            <el-form-item label="更新时间">
              <Datetime :value="value.updatedAt" />
            </el-form-item>
            <el-form-item label="注册时间">
              <Datetime :value="value.createdAt" />
            </el-form-item>
          </el-form>
        </el-card>
      </el-col>
      <el-col :md="12" :sm="24">
        <el-card shadow="never">
          <template #header>
            <span>用户角色</span>
          </template>
          <el-tag
            v-if="roles.length"
            v-for="role in roles"
            :key="role.id"
            style="margin-bottom: 8px; margin-right: 8px"
          >
            {{ role.name }}
          </el-tag>
          <el-empty v-else />
        </el-card>
      </el-col>
    </el-row>
  </div>
</template>

<script setup lang="ts">
  import { RoleDto, RoleService, UserWithDetailsDto } from '@/api/services'

  const { value } = defineProps<{
    value: UserWithDetailsDto
  }>()
  const roles = ref<RoleDto[]>([])
  onMounted(async () => {
    roles.value = await RoleService.getList()
  })
</script>

<style scoped lang="scss">
  .warp {
    display: flex;
    flex-direction: column;
    align-items: center;
  }
</style>
