<template>
  <div v-loading="!user" class="user-details">
    <el-row :gutter="16">
      <el-col :md="12" :sm="24">
        <Basic v-if="user" :value="user" />
      </el-col>
      <el-col :md="12" :sm="24">
        <Ancestors v-if="user" :user-id="user.id" />
      </el-col>
    </el-row>
  </div>
</template>

<script setup lang="ts">
  import { ref, onMounted } from 'vue'
  import { useRoute } from 'vue-router'
  import { UserService, UserWithDetailsDto } from '@/api/services'
  import Basic from './Basic.vue'
  import Ancestors from './Ancestors.vue'

  const route = useRoute()
  const userId = route.params.id as string
  const user = ref<UserWithDetailsDto>()

  onMounted(async () => {
    try {
      user.value = await UserService.get(userId)
    } catch (error) {
      console.error('获取用户信息失败:', error)
    }
  })
</script>

<style scoped lang="scss">
  .user-details {
    .panel {
      margin-bottom: 16px;
      transition: all 0.3s ease;

      &:hover {
        box-shadow: 0 4px 12px rgba(0, 0, 0, 0.1);
      }
    }
  }

  // 响应式设计
  @media (max-width: 768px) {
    .user-details {
      .panel {
        margin-bottom: 12px;
      }
    }
  }
</style>
